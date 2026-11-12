#if __arm64__
#define FRAME_LENGTH 16
#define OBJCMSGFUNC _xamarin_dyn_objc_msgSend_16
#define OBJCMSGCALL _objc_msgSend
#include "trampolines-arm64-objc_msgSend.inc.s"
#endif
