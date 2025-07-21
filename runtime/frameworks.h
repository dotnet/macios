#if TARGET_OS_IOS || TARGET_OS_TV
#define HAVE_UIKIT 1
#else
#define HAVE_UIKIT 0
#endif

#if TARGET_OS_IOS || TARGET_OS_TV || defined (__x86_64__) || defined (__aarch64__)
#define HAVE_MAPKIT 1
#else
#define HAVE_MAPKIT 0
#endif

#define HAVE_COREMEDIA 1

#define HAVE_COREANIMATION 1
