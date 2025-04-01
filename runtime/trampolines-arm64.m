
#if defined(__arm64__)

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
#include "trampolines-arm64.h"
#include "product.h"

/*
 * https://developer.apple.com/library/archive/documentation/Xcode/Conceptual/iPhoneOSABIReference/Articles/ARM64FunctionCallingConventions.html
 * https://github.com/ARM-software/abi-aa/blob/main/aapcs64/aapcs64.rst
 *
 * Standard arm64 calling convention:
 * Input:
 *   x0 - x7 (x8 too in some cases)
 *   q0 - q7 (simd/floating point)
 * Output:
 *   same as input
 *
 */

#ifdef TRACE
static void
dump_state (struct XamarinCallState *state, const char *prefix)
{
	fprintf (stderr, "DUMP %s type: %llu self: %p SEL: %s sp: 0x%llx\n"
		"    x0: 0x%llx = %lld = (double) %f = (float,float) (%f, %f)\n"
		"    x1: 0x%llx = %lld = (double) %f = (float,float) (%f, %f)\n"
		"    x2: 0x%llx = %lld = (double) %f = (float,float) (%f, %f)\n"
		"    x3: 0x%llx = %lld = (double) %f = (float,float) (%f, %f)\n"
		"    x4: 0x%llx = %lld = (double) %f = (float,float) (%f, %f)\n"
		"    x5: 0x%llx = %lld = (double) %f = (float,float) (%f, %f)\n"
		"    x6: 0x%llx = %lld = (double) %f = (float,float) (%f, %f)\n"
		"    x7: 0x%llx = %lld = (double) %f = (float,float) (%f, %f)\n"
		"    x8: 0x%llx = %lld = (double) %f = (float,float) (%f, %f)\n"
		"    long double: q0: %Lf q1: %Lf q2: %Lf q3: %Lf q4: %Lf q5: %Lf q6: %Lf q7: %Lf\n"
		"    double:      q0.d1: %f q0.d2: %f q1.d1: %f q1.d2: %f q2.d1: %f q2.d2: %f q3.d1: %f q3.d2: %f q4.d1: %f q4.d2: %f q5.d1: %f q5.d2: %f q6.d1: %f q6.d2: %f q7.d1: %f q7.d2: %f\n"
		"    float:       q0.f1: %f q0.f2: %f q0.f3: %f q0.f4: %f q1.f1: %f q1.f2: %f q1.f3: %f q1.f4: %f q2.f1: %f q2.f2: %f q2.f3: %f q2.f4: %f q3.f1: %f q3.f2: %f q3.f3: %f q3.f4: %f q4.f1: %f q4.f2: %f q4.f3: %f q4.f4: %f q5.f1: %f q5.f2: %f q5.f3: %f q5.f4: %f q6.f1: %f q6.f2: %f q6.f3: %f q6.f4: %f q7.f1: %f q7.f2: %f q7.f3: %f q7.f4: %f\n",
		prefix, state->type, state->self (), sel_getName (state->sel ()), state->sp,
		state->x0, (uint64_t) state->x0, *(double *) &state->x0, ((float *) &state->x0)[0], ((float *) &state->x0) [1],
		state->x1, (uint64_t) state->x1, *(double *) &state->x1, ((float *) &state->x1)[0], ((float *) &state->x1) [1],
		state->x2, (uint64_t) state->x2, *(double *) &state->x2, ((float *) &state->x2)[0], ((float *) &state->x2) [1],
		state->x3, (uint64_t) state->x3, *(double *) &state->x3, ((float *) &state->x3)[0], ((float *) &state->x3) [1],
		state->x4, (uint64_t) state->x4, *(double *) &state->x4, ((float *) &state->x4)[0], ((float *) &state->x4) [1],
		state->x5, (uint64_t) state->x5, *(double *) &state->x5, ((float *) &state->x5)[0], ((float *) &state->x5) [1],
		state->x6, (uint64_t) state->x6, *(double *) &state->x6, ((float *) &state->x6)[0], ((float *) &state->x6) [1],
		state->x7, (uint64_t) state->x7, *(double *) &state->x7, ((float *) &state->x7)[0], ((float *) &state->x7) [1],
		state->x8, (uint64_t) state->x8, *(double *) &state->x8, ((float *) &state->x8)[0], ((float *) &state->x8) [1],
		state->q0.q, state->q1.q, state->q2.q, state->q3.q, state->q4.q, state->q5.q, state->q6.q, state->q7.q,
		state->q0.d.d1, state->q0.d.d2, state->q1.d.d1, state->q1.d.d2, state->q2.d.d1, state->q2.d.d2, state->q3.d.d1, state->q3.d.d2, state->q4.d.d1, state->q4.d.d2, state->q5.d.d1, state->q5.d.d2, state->q6.d.d1, state->q6.d.d2, state->q7.d.d1, state->q7.d.d2,
		state->q0.f.f1, state->q0.f.f2, state->q0.f.f3, state->q0.f.f4, state->q1.f.f1, state->q1.f.f2, state->q1.f.f3, state->q1.f.f4, state->q2.f.f1, state->q2.f.f2, state->q2.f.f3, state->q2.f.f4, state->q3.f.f1, state->q3.f.f2, state->q3.f.f3, state->q3.f.f4, state->q4.f.f1, state->q4.f.f2, state->q4.f.f3, state->q4.f.f4, state->q5.f.f1, state->q5.f.f2, state->q5.f.f3, state->q5.f.f4, state->q6.f.f1, state->q6.f.f2, state->q6.f.f3, state->q6.f.f4, state->q7.f.f1, state->q7.f.f2, state->q7.f.f3, state->q7.f.f4);
}
#else
#define dump_state(...)
#endif

static int
param_read_primitive (struct ParamIterator *it, const char *type_ptr, uint8_t **target_ptr, size_t total_size, bool prohibit_fp_registers, bool is_struct, bool is_hfa, bool read_register, GCHandle *exception_gchandle)
{
	char type = *type_ptr;
	void *target = *target_ptr;

	// compute size
	size_t size = xamarin_get_primitive_size (type);

	LOGZ ("        reading primitive of type '%c' with size %i total size: %i nsrn: %i ngrn: %i ngrn_offset: %zu nsaa: %p prohibit_fp_registers: %i read_register: %i\n",
			type, (int) size, (int) total_size, it->nsrn, it->ngrn, it->ngrn_offset, it->nsaa, (int) prohibit_fp_registers, (int) read_register);

	if (size == 0)
		return 0;

	// align output pointer to the alignment of the type of the value we're reading
	*target_ptr = (uint8_t *) align_ptr (target, size);
	target = *target_ptr;

	if (type == _C_FLT && !prohibit_fp_registers && it->nsrn < 8) {
		// if we're a float, and we can use floating-point registers, and we haven't used up all the fp registers, then read from the fp registers
		if (target != NULL) {
			*(float *) target = it->q [it->nsrn].f.f1;
			LOGZ ("            reading float at register q%i into %p: %f\n", it->nsrn, target, *(float *) target);
		}
		it->nsrn++;
	} else if (type == _C_DBL && !prohibit_fp_registers && it->nsrn < 8) {
		// if we're a double, and we can use floating-point registers, and we haven't used up all the fp registers, then read from the fp registers
		if (target != NULL) {
			*(double *) target = it->q [it->nsrn].d.d1;
			LOGZ ("            reading double at register q%i into %p: %f\n", it->nsrn, target, *(double *) target);
		}
		it->nsrn++;
	} else {
		// A floating point value can be read from a general register if we're reading it as a part of a struct, but not if it's a single floating point value
		if ((type == _C_FLT || type == _C_DBL) && total_size == size) {
			read_register = false;
		} else if (is_hfa) {
			// HFA's are never read from a general purpose register
			read_register = false;
		}

		uint8_t *ptr;
		if (read_register && it->ngrn_offset > 0) {
			// align the offset we're supposed to read from with the size of the current read
			it->ngrn_offset = align_size (it->ngrn_offset, size);

			// if we'd read past the end of the current register, advance to the next register
			if (it->ngrn_offset + size > 8) {
				LOGZ ("            advancing to the next register (or stack if no more registers), because reading size=%i at offset %i would read past the current register.\n", (int) size, (int) it->ngrn_offset);
				it->ngrn++;
				it->ngrn_offset = 0;
				read_register = it->ngrn < 8;
			}
		}

		if (read_register) {
			ptr = it->ngrn_offset + (uint8_t *) &it->x [it->ngrn];
			if (target != NULL) {
				LOGZ ("            reading from register x%i with offset %i into %p: ", it->ngrn, (int) it->ngrn_offset, target);
			}

			it->ngrn_offset += size;
		} else {
			// align the pointer we're supposed to read from with the size of the current read
			it->nsaa = (uint8_t *) align_ptr (it->nsaa, size);
			ptr = (uint8_t *) it->nsaa;
			if (target != NULL) {
				LOGZ ("            reading from stack at %p into %p: ", ptr, target);
			}
			it->nsaa += size;
		}

		if (target == NULL) {
			LOGZ ("            not reading, since target is NULL.\n");
			return (int) size;
		}

		// do the actual read
		switch (size) {
		case 8:
			*(uint64_t *) target = *(uint64_t *) ptr;
			LOGZ ("0x%llx = %llu = %f\n", * (uint64_t *) target, * (uint64_t *) target, *(double *) target);
			break;
		case 4:
			*(uint32_t *) target = *(uint32_t *) ptr;
			LOGZ ("0x%x = %u = %f\n", * (uint32_t *) target, * (uint32_t *) target, *(float *) target);
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
			*exception_gchandle = xamarin_create_mt_exception (xamarin_strdup_printf (PRODUCT ": Cannot marshal parameter type %c (size: %i): invalid size.\n", type, (int) size));
			return 0;
		}
	}

	return (int) size;
}

static void
param_iter_next (enum IteratorAction action, void *context, const char *type, size_t size, void *target, GCHandle *exception_gchandle)
{
	struct ParamIterator *it = (struct ParamIterator *) context;

	if (action == IteratorStart) {
		it->ngrn = 2; // we already have two arguments: self + SEL
		it->nsrn = 0;
		it->ngrn_offset = 0;
		it->nsaa = (uint8_t *) it->state->sp;
		it->x = &it->state->x0;
		it->q = &it->state->q0;
		LOGZ ("    initialized parameter iterator. next register: %i next fp register: %i next stack pointer: %p\n", it->ngrn, it->nsrn, it->nsaa);
		return;
	} else if (action == IteratorEnd) {
		return;
	}

	// if the previous parameter ended up reading a partial register, advance to the next register
	if (it->ngrn_offset > 0) {
		it->ngrn++;
		it->ngrn_offset = 0;
	}

	// target must be at least pointer sized, and we need to zero it out first.
	if (target != NULL)
		*(uint64_t *) target = 0;

	char struct_name [33]; // any struct with > 32 bytes (4 doubles) is always passed on the stack, which also means any struct with more than 32 fields will be passed on the stack.
	xamarin_collapse_struct_name (type, struct_name, sizeof (struct_name), exception_gchandle);
	if (*exception_gchandle != INVALID_GCHANDLE)
		return;

	// a struct with only 2-4 floats or 2-4 doubles (but not a 2-4 mix of floats and doubles) can be passed in floating point registers.
	size_t struct_length = strlen (struct_name);
	size_t field_count = struct_length;
	bool is_struct = field_count > 1;

	// hfa = "A Homogeneous Floating-point Aggregate (HFA) is a Homogeneous Aggregate with a Fundamental Data Type that is a Floating-Point type and at most four uniquely addressable members."
	bool is_hfa = is_struct && struct_length >= 2 && struct_length <= 4;
	if (is_hfa) {
		is_hfa = struct_name [0] == 'f' || struct_name [0] == 'd';
		for (int i = 1; i < struct_length; i++)
			is_hfa &= struct_name [i] == struct_name [0];
	}

	int ngrn_registers_left = 8 - it->ngrn;
	bool read_register = ngrn_registers_left > 0;

	if (size > 16 && !is_hfa) {
		LOGZ ("    reading parameter passed by reference. type: %s size: %i next register: %i next fp register: %i nsaa: %p\n", type, (int) size, it->ngrn, it->nsrn, it->nsaa);
		param_read_primitive (it, "^", (uint8_t **) &target, sizeof (void *), true, false, false, read_register, exception_gchandle);
		return;
	}

	if (*struct_name == 0) {
		*exception_gchandle = xamarin_create_mt_exception (xamarin_strdup_printf (PRODUCT ": Cannot marshal parameter type %s, collapsed type name is empty\n", type));
		return;
	}

	// passed in registers (and on the stack if not enough registers)
	LOGZ ("    reading parameter from registers/stack. type: %s (collapsed: %s) size: %i is_hfa: %i\n", type, struct_name, (int) size, is_hfa);
	const char *t = struct_name;
	uint8_t *targ = (uint8_t *) target;

	// members of structs are generally not passed in floating point registers (except for is_hfa structs, see above)
	// here we detect a struct by checking if we have more than one field to read
	bool prohibit_fp_registers = !is_hfa && is_struct;

	if (is_hfa) {
		// if the current hfa doesn't fit into the remaining floating point registers, nothing more will go into floating point registers
		if (8 - it->nsrn < field_count) {
			it->nsrn = 8;
			read_register = false;
		}
	} else if (read_register) {
		// compute struct size including padding
		// detect if the current parameter fully fits in the general registers
		// if not, disable any more reading from the general registers
		size_t ssize = 0;
		for (int i = 0; struct_name [i] != 0; i++) {
			size_t fsize = xamarin_get_primitive_size (struct_name [i]);
			ssize = align_size (ssize, fsize);
			ssize += fsize;
		}
		size_t required_registers = (ssize + 7) / 8;
		read_register = ngrn_registers_left >= required_registers;
		LOGZ ("        ngrn registers left: %i required registers: %i read_register: %i\n", (int) ngrn_registers_left, (int) required_registers, (int) read_register);
		if (!read_register)
			it->ngrn = 8; // nothing more will be read from registers
	}

	do {
		int c = param_read_primitive (it, t, &targ, size, prohibit_fp_registers, is_struct, is_hfa, read_register, exception_gchandle);
		if (*exception_gchandle != INVALID_GCHANDLE)
			return;
		if (targ != NULL)
			targ += c;
	} while (*++t);

	// align stack reads to sizeof(void*) bytes
	it->nsaa = (uint8_t *) align_ptr (it->nsaa, 8);
}

static void
marshal_return_value (void *context, const char *type, size_t size, void *vvalue, MonoType *mtype, bool retain, MonoMethod *method, MethodDescription *desc, GCHandle *exception_gchandle)
{
	char struct_name[5];
	MonoObject *value = (MonoObject *) vvalue;
	struct ParamIterator *it = (struct ParamIterator *) context;

	LOGZ ("    marshalling return value %p as %s with size %i\n", value, type, (int) size);

	switch (type [0]) {
	case _C_FLT:
		// single floating point return value
		it->state->q0.f.f1 = *(float *) mono_object_unbox (value);
		break;
	case _C_DBL:
		// double floating point return value
		it->state->q0.q = *(double *) mono_object_unbox (value);
		break;
	case _C_STRUCT_B:
		/*
		 * Structures, this is ugly, but not as bad as x86_64.
		 *
		 * A) Structures of 1 to 4 fields of the same floating point type (float or double), are passed in the q[0-3] registers.
		 * B) Any other structure > 16 bytes is passed by reference (pointer to memory in x8)
		 * C) Structures <= 16 bytes are passed in x0 and x1 as needed. Each register can contain multiple fields.
		 */

		xamarin_collapse_struct_name (type, struct_name, sizeof (struct_name), exception_gchandle);
		if (*exception_gchandle != INVALID_GCHANDLE)
			return;

		if ((size == 32 && !strncmp (struct_name, "dddd", 4)) ||
			(size == 24 && !strncmp (struct_name, "ddd", 3)) ||
			(size == 16 && !strncmp (struct_name, "dd", 2)) ||
			(size == 8 && !strncmp (struct_name, "d", 1))) {
			LOGZ ("        marshalling as %i doubles (struct name: %s)\n", (int) size / 8, struct_name);
			double* ptr = (double *) mono_object_unbox (value);
			for (int i = 0; i < size / 8; i++) {
				LOGZ ("        #%i: %f\n", i, ptr [i]);
				it->q [i].q = ptr [i];
			}
		} else if ((size == 16 && !strncmp (struct_name, "ffff", 4)) ||
				   (size == 12 && !strncmp (struct_name, "fff", 3)) ||
				   (size == 8 && !strncmp (struct_name, "ff", 2)) ||
				   (size == 4 && !strncmp (struct_name, "f", 1))) {
			LOGZ ("        marshalling as %i floats (struct name: %s)\n", (int) size / 4, struct_name);
			float* ptr = (float *) mono_object_unbox (value);
			for (int i = 0; i < size / 4; i++) {
				LOGZ ("        #%i: %f\n", i, ptr [i]);
				it->q [i].f.f1 = ptr [i];
			}
		} else if (size > 16) {
			LOGZ ("        marshalling as stret through x8\n");
			memcpy ((void *) it->state->x8, mono_object_unbox (value), size);
		} else {
			LOGZ ("        marshalling as composite values in x0 and x1\n");
			it->state->x0 = 0;
			it->state->x1 = 0;
			memcpy (&it->state->x0, mono_object_unbox (value), size);
		}
		break;
	// For primitive types we get a pointer to the actual value
	case _C_BOOL: // signed char
	case _C_CHR:
	case _C_UCHR:
		it->state->x0 = *(uint8_t *) mono_object_unbox (value);
		break;
	case _C_SHT:
	case _C_USHT:
		it->state->x0 = *(uint16_t *) mono_object_unbox (value);
		break;
	case _C_INT:
	case _C_UINT:
		it->state->x0 = *(uint32_t *) mono_object_unbox (value);
		break;
	case _C_LNG:
	case _C_ULNG:
	case _C_LNG_LNG:
	case _C_ULNG_LNG:
		it->state->x0 = *(uint64_t *) mono_object_unbox (value);
		break;

	// For pointer types we get the value itself.
	case _C_CLASS:
	case _C_SEL:
	case _C_ID:
	case _C_CHARPTR:
	case _C_PTR:
		if (value == NULL) {
			it->state->x0 = 0;
			break;
		}

		it->state->x0 = (uint64_t) xamarin_marshal_return_value (it->state->sel (), mtype, type, value, retain, method, desc, exception_gchandle);
		break;
	case _C_VOID:
		break;
	case '|': // direct pointer value
	default:
		if (size == 8) {
			it->state->x0 = (uint64_t) value;
		} else {
			*exception_gchandle = xamarin_create_mt_exception (xamarin_strdup_printf (PRODUCT ": Cannot marshal return type %s (size: %i)\n", type, (int) size));
		}
		break;
	}
}

void
xamarin_arch_trampoline (struct XamarinCallState *state)
{
	state->_self = (id) state->x0;
	state->_sel = (SEL) state->x1;

	dump_state (state, "BEGIN: ");
	struct ParamIterator iter;
	iter.state = state;
	xamarin_invoke_trampoline ((enum TrampolineType) state->type, state->self (), state->sel (), param_iter_next, marshal_return_value, &iter);
	dump_state (state, "END:   ");
	LOGZ ("\n");
}

bool
xamarin_arch_param_passed_by_reference (unsigned long size, const char *type, GCHandle *exception_gchandle)
{
	if (size <= 16)
		return false;

	char struct_name [5]; // we don't care about structs with more than 4 fields.
	GCHandle exc = INVALID_GCHANDLE;
	xamarin_collapse_struct_name (type, struct_name, sizeof (struct_name), &exc);
	if (exc != INVALID_GCHANDLE) {
		// if we failed to collapse the struct, it's because it can't be a struct with size>16 that we can pass in floating point registers
		// which means it has to be a normal struct with size>16, which is passed by reference.
		// Note: this exception is not bubbled up, because it's not a failure scenario.
		xamarin_gchandle_free (exc);
		return true;
	}

	if (strcmp (struct_name, "dddd") == 0)
		return false;

	if (strcmp (struct_name, "ddd") == 0)
		return false;

	return true;
}

#endif /* __arm64__ */
