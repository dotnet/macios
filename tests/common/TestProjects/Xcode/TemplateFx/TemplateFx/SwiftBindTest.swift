//
//  TemplateFx.swift
//  TemplateFx
//

import Foundation
import DynamicDependency

@objc(SwiftBindTest)
public class SwiftBindTest : NSObject
{
    @objc
    public static func getString(myString: String) -> String {
        return myString  + dependencyMessage()
    }

    //REPLACE

}
