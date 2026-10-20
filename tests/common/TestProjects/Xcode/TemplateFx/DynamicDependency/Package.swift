// swift-tools-version: 5.9
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

import PackageDescription

let package = Package(
    name: "DynamicDependency",
    products: [
        .library(name: "DynamicDependency", type: .dynamic, targets: ["DynamicDependency"]),
    ],
    targets: [
        .target(name: "DynamicDependency"),
    ]
)
