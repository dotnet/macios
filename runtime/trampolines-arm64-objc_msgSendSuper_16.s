#if __arm64__
#define FRAME_LENGTH 16
#define OBJCMSGFUNC _xamarin_dyn_objc_msgSendSuper_16
#define OBJCMSGCALL _objc_msgSendSuper
#include "trampolines-arm64-objc_msgSend.inc.s"
#endif
