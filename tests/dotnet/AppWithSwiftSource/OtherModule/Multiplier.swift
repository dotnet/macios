#if !APP_WITH_SWIFT_SOURCE_DEFINE
#error("The AdditionalArguments metadata wasn't passed to swiftc")
#endif

@_cdecl("swift_multiply")
public func swiftMultiply(_ a: Int32, _ b: Int32) -> Int32 {
	return a * b * bridged_one()
}
