#if defined(__x86_64__)

#include <stdint.h>
#include <stdlib.h>
#include <stdio.h>
#include <string.h>

#include <objc/objc.h>
#include <objc/runtime.h>
#include <objc/message.h>

#include "trampolines-internal.h"
#include "xamarin/runtime.h"
#include "runtime-internal.h"
#include "trampolines-x86_64.h"
#include "product.h"

/*
 * Standard x86_64 calling convention:
 * Input:
 *   %rdi, %rsi, %rdx, %rcx, %r8, %r9
 *   %xmm0 - %xmm7
 * Output:
 *   %rax
 *   %xmm0 - %xmm1
 *
 * Preserved registers: %rbp, %rbx, %r12-%r15
 *
 * Stack:
 *  16(%rbp): stack-allocated parameters
 *   8(%rbp): return address
 *   0(%rbp): previous %rbp value
 */

#ifdef TRACE
static void
dump_state (const char *prefix, struct XamarinCallState *state)
{
	fprintf (stderr, "DUMP %s type: %llu is_stret: %i self: %p SEL: %s rdi: 0x%llx rsi: 0x%llx rdx: 0x%llx rcx: 0x%llx r8: 0x%llx r9: 0x%llx rbp: 0x%llx rax: 0x%llx rdx out: 0x%llx\n"
		"    long double: xmm0: %Lf xmm1: %Lf xmm2: %Lf xmm3: %Lf xmm4: %Lf xmm5: %Lf xmm6: %Lf xmm7: %Lf\n"
		"    double:      xmm0[0]: %f xmm0[1]: %f xmm1[0]: %f xmm1[1]: %f xmm2[0]: %f xmm2[1]: %f xmm3[0]: %f xmm3[1]: %f xmm4[0]: %f xmm4[1]: %f xmm5[0]: %f xmm5[1]: %f xmm6[0]: %f xmm6[1]: %f xmm7[0]: %f xmm7[1]: %f\n"
		"    float:       xmm0[0]: %f xmm0[1]: %f xmm0[2]: %f xmm0[3]: %f xmm1[0]: %f xmm1[1]: %f xmm1[2]: %f xmm1[3]: %f xmm2[0]: %f xmm2[1]: %f xmm2[2]: %f xmm2[3]: %f xmm3[0]: %f xmm3[1]: %f xmm3[2]: %f xmm3[3]: %f xmm4[0]: %f xmm4[1]: %f xmm4[2]: %f xmm4[3]: %f xmm5[0]: %f xmm5[1]: %f xmm5[2]: %f xmm5[3]: %f xmm6[0]: %f xmm6[1]: %f xmm6[2]: %f xmm6[3]: %f xmm7[0]: %f xmm7[1]: %f xmm7[2]: %f xmm7[3]: %f\n",
		prefix, state->type, state->is_stret (), state->self (), sel_getName (state->sel ()), state->rdi, state->rsi, state->rdx, state->rcx, state->r8, state->r9, state->rbp, state->rax, state->rdx_out,
		state->xmm0, state->xmm1, state->xmm2, state->xmm3, state->xmm4, state->xmm5, state->xmm6, state->xmm7,
		((double *) &state->xmm0)[0], ((double *) &state->xmm0)[1], ((double *) &state->xmm1)[0], ((double *) &state->xmm1)[1], ((double *) &state->xmm2)[0], ((double *) &state->xmm2)[1], ((double *) &state->xmm3)[0], ((double *) &state->xmm3)[1], ((double *) &state->xmm4)[0], ((double *) &state->xmm4)[1], ((double *) &state->xmm5)[0], ((double *) &state->xmm5)[1], ((double *) &state->xmm6)[0], ((double *) &state->xmm6)[1], ((double *) &state->xmm7)[0], ((double *) &state->xmm7)[1],
		((float *) &state->xmm0)[0], ((float *) &state->xmm0)[1], ((float *) &state->xmm0)[2], ((float *) &state->xmm0)[3], ((float *) &state->xmm1)[0], ((float *) &state->xmm1)[1], ((float *) &state->xmm1)[2], ((float *) &state->xmm1)[3], ((float *) &state->xmm2)[0], ((float *) &state->xmm2)[1], ((float *) &state->xmm2)[2], ((float *) &state->xmm2)[3], ((float *) &state->xmm3)[0], ((float *) &state->xmm3)[1], ((float *) &state->xmm3)[2], ((float *) &state->xmm3)[3], ((float *) &state->xmm4)[0], ((float *) &state->xmm4)[1], ((float *) &state->xmm4)[2], ((float *) &state->xmm4)[3], ((float *) &state->xmm5)[0], ((float *) &state->xmm5)[1], ((float *) &state->xmm5)[2], ((float *) &state->xmm5)[3], ((float *) &state->xmm6)[0], ((float *) &state->xmm6)[1], ((float *) &state->xmm6)[2], ((float *) &state->xmm6)[3], ((float *) &state->xmm7)[0], ((float *) &state->xmm7)[1], ((float *) &state->xmm7)[2], ((float *) &state->xmm7)[3]
		);
}
#else
#define dump_state(...)
#endif

#ifdef TRACE
static const char* registers[] =  { "rdi", "rsi", "rdx", "rcx", "r8", "r9", "err"  };
#endif

const unsigned long total_register_size = 48; // 48 == 6 registers * 8 bytes

static unsigned long 
param_read_primitive (struct ParamIterator *it, const char **type_ptr, uint8_t **target_ptr, size_t total_size, bool fp_struct, bool float_in_register_candidate, bool read_register, GCHandle *exception_gchandle)
{
	// COOP: does not access managed memory: any mode.
	char type = **type_ptr;
	void *target = *target_ptr;

	LOGZ (" reading primitive: type=%c target_ptr=%p target=%p total_size=%i fp_struct=%i float_in_register_candidate=%i read_register=%i it->byte_count=%i it->float_count=%i\n",
			type, target_ptr, target, (int) total_size, (int) fp_struct, (int) float_in_register_candidate, (int) read_register, it->byte_count, it->float_count);

	// compute size
	size_t size = xamarin_get_primitive_size (type);

	// align output pointer to the alignment of the type of the value we're reading
	*target_ptr = (uint8_t *) align_ptr (target, size);
	target = *target_ptr;

	switch (type) {
	case _C_FLT: {
		// there are 8 xmm registers, each 4 floats big.
		// only 2 floats are ever put in each xmm register.
		// this means the last location is 7 * 4 + 1 = 29
		if (total_size > size && !fp_struct && !float_in_register_candidate) {
			// floating point values in a struct that has non-floating point fields are not stored in floating point registers.
			goto DEFAULT;
		} else if ((read_register || fp_struct) && it->float_count <= 29) {
			if (target != NULL) {
				*(float*) target = *(it->float_count + (float*) &it->state->xmm0);
				LOGZ (" reading float at xmm%i (%s half) into %p: %f\n", it->float_count / 4, it->float_count % 2 ? "second" : "first", target, *(float *) target);
			}
			it->float_count++;
			if (it->float_count % 2 == 0)
				it->float_count += 2; // only the first 64 bits of the 128-bit long xmm registers are used, so skip the last 64 bits.
		} else {
			it->stack_next = (uint8_t *) align_ptr (it->stack_next, size); // align to natural alignment for the value we want to read
			if (target != NULL) {
				*(float*) target = *(float*) it->stack_next;
				LOGZ (" reading float at stack %p into %p: %f\n", it->stack_next, target, *(float*)target);
			}
			it->stack_next += 4;
		}
		break;
	}
	case _C_DBL: {
		if (it->float_count % 4 != 0) {
			it->float_count = align_int32 (it->float_count, 4);
			LOGZ (" advancing float pointer to read double value from full register\n");
		}
		// there are 8 xmm registers, each 4 floats big.
		// only 1 double is ever put in each xmm register.
		// this means the last location is 7 * 4 = 28
		/*if (total_size > size && !fp_struct && !read_register) {
			// floating point values in a struct that has non-floating point fields are not stored in floating point registers.
			goto DEFAULT;
		} else */if (it->float_count <= 28) {
			if (target != NULL) {
				*(double*) target = *(double*) (it->float_count + (float*) &it->state->xmm0);
				LOGZ (" reading double at xmm%i = %p into %p: %f\n", it->float_count / 4, (double *) target, target, *(double *) target);
			}
			it->float_count += 4; // each xmm register is 128-bit = 4 floats
		} else {
			it->stack_next = (uint8_t *) align_ptr (it->stack_next, size); // align to natural alignment for the value we want to read
			if (target != NULL) {
				*(double*) target = *(double*) it->stack_next;
				LOGZ (" reading double at stack %p into %p: %f\n", it->stack_next, target, *(double *) target);
			}
			it->stack_next += 8;
		}
		break;
	}
	case _C_PTR: { // ^
		// Need to skip what's pointed to
		int nesting = 0;
		do {
			(*type_ptr)++;
			if (**type_ptr == '{')
				nesting++;
			else if (**type_ptr == '}')
				nesting--;
		} while (**type_ptr != 0 && nesting > 0);
		// fallthrough
	}
DEFAULT:
	default: {
		if (size == 0)
			return 0;

		uint8_t *ptr;
		if (read_register) {
			// align the pointer we're supposed to read from with the size of the current read
			it->byte_count = align_int32 (it->byte_count, (int) size);
			// check if the current register has enough space left for the value we want to read, if not advance to the next register
			if (it->byte_count / 8 != ((unsigned long) it->byte_count + size - 1) / 8) {
				// align to next register if the one we're currently reading
				// doesn't contain the entire value we need.
				it->byte_count = align_int32 (it->byte_count, 8);
				LOGZ (" skipping until next register #%i = %s, can't read primitive of size %i from current register\n", (it->byte_count / 8) + 1, registers [it->byte_count / 8], (int) size);
			}
			 
			ptr = it->byte_count + (uint8_t *) &it->state->rdi;
			if (target != NULL)
				LOGZ (" reading primitive of size %i from register #%i = %s (byte count %i offset %i) at %p into %p; ", (int) size, (it->byte_count / 8) + 1, registers [it->byte_count / 8], it->byte_count, it->byte_count % 8, ptr, target);
			it->byte_count += size;
		} else {
			it->stack_next = (uint8_t *) align_ptr (it->stack_next, size); // align to natural alignment for the value we want to read
			ptr = (uint8_t *) it->stack_next;
			if (target != NULL)
				LOGZ (" reading primitive of size %i from stack (byte count %i offset %i) at %p into %p:  ", (int) size, it->byte_count, it->byte_count % 8, ptr, target);
			it->stack_next += size;
		}

		if (target == NULL)
			return size;

		switch (size) {
		case 8:
			*(uint64_t *) target = *(uint64_t *) ptr;
			LOGZ ("0x%llx = %llu = %f\n", * (uint64_t *) target, * (uint64_t *) target, * (double *) target);
			break;
		case 4:
			*(uint32_t *) target = *(uint32_t *) ptr;
			LOGZ ("0x%x = %u = %f\n", * (uint32_t *) target, * (uint32_t *) target, * (float *) target);
			break;
		case 2:
			*(uint16_t *) target = *(uint16_t *) ptr;
			LOGZ ("0x%x = %u\n", (int) * (uint32_t *) target, (int) * (uint32_t *) target);
			break;
		case 1:
			*(uint8_t *) target = *(uint8_t *) ptr;
			LOGZ ("0x%x = %u = '%c'\n", (int) * (uint8_t *) target, (int) * (uint8_t *) target, (char) * (uint8_t *) target);
			break;
		default:
			*exception_gchandle = xamarin_create_mt_exception (xamarin_strdup_printf ("Xamarin.iOS: Cannot marshal parameter type %c (size: %i): invalid size.\n", type, (int) size));
			return 0;
		}
		break;
	}
	}

	return size;
}

static void
param_iter_next (enum IteratorAction action, void *context, const char *type, size_t size, void *target, GCHandle *exception_gchandle)
{
	// COOP: does not access managed memory: any mode.
	struct ParamIterator *it = (struct ParamIterator *) context;

	if (action == IteratorStart) {
		it->byte_count = (it->state->type & Tramp_Stret) == Tramp_Stret ? 24 : 16;
		it->float_count = 0;
		it->stack_next = (uint8_t *) (2 +  (uint64_t *) it->state->rbp);
		LOGZ("initialized parameter iterator. byte count: %i stack next: %p\n", it->byte_count, it->stack_next);
		return;
	} else if (action == IteratorEnd) {
		return;
	}

	// target must be at least pointer sized, and we need to zero it out first.
	if (target != NULL)
		*(uint64_t *) target = 0;

	if (size > 16) {
		// passed on the stack
		if (target != NULL)
			memcpy (target, it->stack_next, size);
		// increment stack pointer
		it->stack_next += size;
		// and round up to 8 bytes.
		if (size % 8 != 0)
			it->stack_next += 8 - size % 8;
		return;
	}

	char struct_name [17]; // any struct with > 16 bytes is passed on the stack, which also means any struct with more than 16 fields will be passed on the stack.
	xamarin_collapse_struct_name (type, struct_name, sizeof (struct_name), exception_gchandle);
	if (*exception_gchandle != INVALID_GCHANDLE)
		return;

	// a struct only floats or doubles can be passed in floating point registers.
	size_t struct_length = strlen (struct_name);
	bool fp_struct = true; // struct_length >= 2; // not a struct if only a single field
	if (fp_struct) {
		for (int i = 0; i < struct_length; i++)
			fp_struct &= struct_name [i] == 'f' || struct_name [i] == 'd';
	}

	if (*struct_name == 0) {
		*exception_gchandle = xamarin_create_mt_exception (xamarin_strdup_printf (PRODUCT ": Cannot marshal parameter type %s, collapsed type name is empty\n", type));
		return;
	}

	bool read_register = true;
	if (total_register_size - (size_t) it->byte_count < size) {
		read_register = false;
		LOGZ (" total size of parameter (%i) is less than size of remaining registers (%i), so this argument will be passed on the stack\n", (int) size, (int) ((int) total_register_size - it->byte_count));
	}

	// passed in registers (and on the stack if not enough registers)
	const char *t = struct_name;
	unsigned long current_size = 0;
	bool float_in_register_candidate = false;
	uint8_t *targ = (uint8_t *) target;
	do {
		if (current_size % 8 == 0) {
			const char *ts = t;
			int bytesConsumed = 0;
			float_in_register_candidate = true;
			do {
				switch (*ts) {
				case _C_DBL:
					bytesConsumed += 8;
					break;
				case _C_FLT:
					bytesConsumed += 4;
					break;
				default:
					float_in_register_candidate = false;
					break;
				}
			} while (*++ts && float_in_register_candidate && bytesConsumed < 8);
		}

		uint8_t *previous_target = targ;
		unsigned long c = param_read_primitive (it, &t, &targ, size, fp_struct, float_in_register_candidate, read_register, exception_gchandle);
		if (*exception_gchandle != INVALID_GCHANDLE)
			return;
		if (targ != NULL)
			targ += c;
		size_t target_diff = (size_t) (targ - previous_target);
		current_size += target_diff;
	} while (*++t);

	// Each argument uses at least one full register, so round up.
	it->float_count = align_int32 (it->float_count, 4);
	it->byte_count = align_int32 (it->byte_count, 8);

	// All arguments use at least 8 bytes, so round up.
	it->stack_next = (uint8_t *) align_ptr (it->stack_next, 8);

#ifdef TRACE
	LOGZ (" output buffer of size %i:", (int) size);
	if (size > 8)
			LOGZ ("\n     ");
	for (int i = 0; i < size; i++) {
		if (i > 0 && i % 8 == 0)
			LOGZ ("\n     ");
		LOGZ (" 0x%.2x", ((uint8_t *) target) [i]);
	}
	LOGZ ("\n");
#endif
}

static void
marshal_return_value (void *context, const char *type, size_t size, void *vvalue, MonoType *mtype, bool retain, MonoMethod *method, MethodDescription *desc, GCHandle *exception_gchandle)
{
	// COOP: accessing managed memory (as input), so must be in unsafe mode.
	MONO_ASSERT_GC_UNSAFE;
	
	MonoObject *value = (MonoObject *) vvalue;
	struct ParamIterator *it = (struct ParamIterator *) context;

	LOGZ (" marshalling return value %p as %s with size=%i\n", value, type, (int) size);

	it->state->xmm0 = 0;
	it->state->xmm1 = 0;

	switch (type [0]) {
	case _C_FLT:
		// single floating point return value
		*(float*)&it->state->xmm0 = *(float *) mono_object_unbox (value);
		break;
	case _C_DBL:
		// double floating point return value
		*(double*)&it->state->xmm0 = *(double *) mono_object_unbox (value);
		break;
	case _C_STRUCT_B:
		/*
		 * Structures, this is ugly :|
		 *
		 * Fortunately we don't have to implement support for the full x86_64 ABI, since we don't need
		 * to support all the types. We only have to implement support for two classes of types:
		 * 
		 *   INTEGER: all variants of ints/uints/pointers. IOW anything that fits into a pointer-sized variable and isn't a floating point value.
		 *   FLOAT:   float, double.
		 * 
		 * To make things more interesting, struct fields are joined together until the reach 64-bit size,
		 * so for instance two int fields will be stuffed into one 64-bit INTEGER register. Same for floats,
		 * two floats will be put into one 64-bit FLOAT register. If there's a mix of floats
		 * and ints the ints win, so a float+int will be put into a 64-bit INTEGER register.
		 * There are also two registers available for each class:
		 * 
		 *   INTEGER: %rax and %rdi
		 *   FLOAT: %xmm0 and %xmm1
		 *
		 * Up to 2 registers (either both INTEGER, both FLOAT or a mix) can be used. This means that 
		 * structs up to 16 bytes can be (and are) passed in registers.
		 * 
		 * A few examples (d=double f=float i=int c=char):
		 * 
		 *	M(d);     // xmm0
		 *	M(dd);    // xmm0 + xmm1
		 *	M(ddd);   // stack
		 *	M(dddd);  // stack
		 *	M(i);     // eax
		 *	M(id);    // eax + xmm0
		 *	M(di);    // xmm0 + eax
		 *	M(ddi);   // stack
		 *	M(ii);    // rax
		 *	M(iii);   // rax + edx
		 *	M(iiii);  // rax + rdx
		 *	M(iiiii); // stack
		 *	M(idi);   // stack
		 *	M(iid);   // rax + xmm0
		 *	M(ll);    // rax + rdx
		 *	M(lll);   // stack
		 *	M(cccc);  // eax
		 *	M(ffff);  // xmm0 + xmm1 
		 *  M(if_);   // rax
		 *  M(f);     // xmm0
		 *  M(iff);   // rax + xmm0 (if: rax, f: xmm0)
		 *  M(iiff);  // rax + xmm0
		 *  M(fi);    // rax
		 * 
		 */
		
		if ((it->state->type & Tramp_Stret) == Tramp_Stret) {
			memcpy ((void *) it->state->rdi, mono_object_unbox (value), size);
			break;
		}

		if (size > 8 && size <= 16) {
			uint64_t *i_ptr = &it->state->rax;
			uint64_t *f_ptr = (uint64_t *) &it->state->xmm0;
			uint64_t *reg_ptr = f_ptr;

			void *unboxed = mono_object_unbox (value);

			// read the struct into 2 64bit values.
			uint64_t v[2] = { 0, 0 };
			memcpy (&v[0], unboxed, size);

			LOGZ (" marshalling two register return value v[0]=%p v[1]=%p i_ptr=%p f_ptr=%p\n", (void *) v[0], (void *) v[1], i_ptr, f_ptr);

			// figure out where to put the values.
			const char *t = xamarin_skip_type_name (type);
			unsigned long acc = 0;
			int stores = 0;

			while (true) {
				if (*t == 0) {
					if (stores >= 2 && acc > 0) {
						*exception_gchandle = xamarin_create_mt_exception (xamarin_strdup_printf ("Xamarin.iOS: Cannot marshal return type %s (size: %i): more than 2 64-bit values found.\n", type, (int) size));
						return;
					} else if (stores < 2) {
						*reg_ptr = v [stores];
					}
					break;
				}
					
				bool is_float = *t == _C_FLT || *t == _C_DBL;
				unsigned long s = xamarin_get_primitive_size (*t);

				t++;

				if (s == 0)
					continue;

				if (acc + s == 8) {
					// We have exactly the amount of data we need for one register.
					// Store the value and start over again.
					reg_ptr = is_float ? reg_ptr : i_ptr;
					acc = 0;
				} else if (acc + s < 8) {
					// We haven't filled up a register yet.
					// Continue iterating.
					reg_ptr = is_float ? reg_ptr : i_ptr;
					acc = align_ulong (acc, s);
					acc += s;
					// find next.
					continue;
				} else {
					// We've overflown. Store the value and start over again,
					// setting the current total to the size of the current type.
					acc = s;
				}

				if (stores >= 2) {
					*exception_gchandle = xamarin_create_mt_exception (xamarin_strdup_printf ("Xamarin.iOS: Cannot marshal return type %s (size: %i): more than 2 64-bit values found.\n", type, (int) size));
					return;
				}

				// Write the current value to the correct register.
				LOGZ ("   store #%i to %p\n", stores, reg_ptr);
				*reg_ptr = v [stores++];
				if (reg_ptr == f_ptr) {
					f_ptr += 2; // xmm0/xmm1 are 128-bit wide (long double).
				} else {
					i_ptr++;
				}

				if (acc == s) {
					// Overflown codepath from above.
					reg_ptr = is_float ? f_ptr : i_ptr;
				} else {
					reg_ptr = f_ptr;
				}

			};
		} else if (size == 8) {
			type = xamarin_skip_type_name (type);
			if (!strncmp (type, "ff}", 3) || !strncmp (type, "d}", 2)) {
				// the only two fully fp combinations are: ff and d
				memcpy (&it->state->xmm0, mono_object_unbox (value), 8);
			} else {
				// all other combinations would contain at least one INTEGER-class type.
				it->state->rax = *(uint64_t *) mono_object_unbox (value);
			}
		} else if (size < 8) {
			type = xamarin_skip_type_name (type);
			if (!strncmp (type, "f}", 2)) {
				memcpy (&it->state->xmm0, mono_object_unbox (value), 4);
			} else {
				memcpy (&it->state->rax, mono_object_unbox (value), size);
			}
		} else {
			*exception_gchandle = xamarin_create_mt_exception (xamarin_strdup_printf ("Xamarin.iOS: Cannot marshal struct return type %s (size: %i)\n", type, (int) size));
			return;
		}
		break;
	// For primitive types we get a pointer to the actual value
	case _C_BOOL: // signed char
	case _C_CHR:
	case _C_UCHR:
		it->state->rax = *(uint8_t *) mono_object_unbox (value);
		break;
	case _C_SHT:
	case _C_USHT:
		it->state->rax = *(uint16_t *) mono_object_unbox (value);
		break;
	case _C_INT:
	case _C_UINT:
		it->state->rax = *(uint32_t *) mono_object_unbox (value);
		break;
	case _C_LNG:
	case _C_ULNG:
	case _C_LNG_LNG:
	case _C_ULNG_LNG:
		it->state->rax = *(uint64_t *) mono_object_unbox (value);
		break;
	
	// For pointer types we get the value itself.
	case _C_CLASS:
	case _C_SEL:
	case _C_ID:
	case _C_CHARPTR:
	case _C_PTR:
		if (value == NULL) {
			it->state->rax = 0;
			break;
		}

		it->state->rax = (uint64_t) xamarin_marshal_return_value (it->state->sel (), mtype, type, value, retain, method, desc, exception_gchandle);
		break;
	case _C_VOID:
		break;
	case '|': // direct pointer value
	default:
		if (size == 8) {
			it->state->rax = (uint64_t) value;
		} else {
			*exception_gchandle = xamarin_create_mt_exception (xamarin_strdup_printf ("Xamarin.iOS: Cannot marshal return type %s (size: %i)\n", type, (int) size));
		}
		break;
	}
}

void
xamarin_arch_trampoline (struct XamarinCallState *state)
{
	// COOP: called from ObjC, and does not access managed memory.
	MONO_ASSERT_GC_SAFE;

	enum TrampolineType type = (enum TrampolineType) state->type;
	dump_state ("PRE ", state);
	struct ParamIterator iter;
	iter.state = state;
	xamarin_invoke_trampoline (type, state->self (), state->sel (), param_iter_next, marshal_return_value, &iter);
	dump_state ("POST", state);
}

bool
xamarin_arch_param_passed_by_reference (unsigned long size, const char *type, GCHandle *exception_gchandle)
{
	// *exception_gchandle = xamarin_create_mt_exception (xamarin_strdup_printf ("Xamarin.iOS: Cannot marshal parameter type %c (size: %i): not implemented for x64.\n", type, (int) size));
	return false;
}

#endif /* __x86_64__ */