#if __arm64__
#define KNOWN_FRAME_LENGTH
#define OBJCMSGFUNC _xamarin_fixed_objc_msgSend
#include "trampolines-arm64-objc_msgSend.inc.s"
#endif
