## Release 0.1.0

### New Rules

| Rule ID | Category | Severity | Notes                                                                                |
|---------|----------|----------|--------------------------------------------------------------------------------------|
| RBI0001 | Usage    | Error    | Binding types should be declared as partial classes.                                 |
| RBI0002 | Usage    | Error    | BindingType<Class> must be on a class.                                               |
| RBI0003 | Usage    | Error    | BindingType<Category> must be on a class.                                            |
| RBI0004 | Usage    | Error    | BindingType<Category> must be on a static class.                                     |
| RBI0005 | Usage    | Error    | BindingType<Protocol> must be on an interface.                                       |
| RBI0006 | Usage    | Error    | BindingType must be on an enumerator.                                                |
| RBI0007 | Usage    | Error    | BindingType<StrongDictionary> must be on a class.                                    |
| RBI0008 | Usage    | Error    | Smart enum values must be tagged with an Field<EnumValue> attribute.                 |
| RBI0009 | Usage    | Error    | Smart enum backing field cannot appear more than once.                               |
| RBI0010 | Usage    | Error    | Smart enum backing field must represent a valid C# identifier to be used.            |
| RBI0011 | Usage    | Error    | Non Apple framework bindings must provide a library name.                            |
| RBI0012 | Usage    | Warning  | Do not provide the LibraryName for known Apple frameworks.                           |
| RBI0013 | Usage    | Error    | Enum values must be tagged with Field<EnumValue>.                                    |
| RBI0014 | Usage    | Error    | Variable has its Handle property accessed but is not kept alive later in the method. |
| RBI0015 | Usage    | Error    | A field is missing when a flag is used in one of the attributes.                     |
| RBI0016 | Usage    | Error    | Fields are mutually exclusive.                                                       |
| RBI0017 | Usage    | Error    | Field used with the wrong flag.                                                      |
| RBI0018 | Usage    | Error    | The export attribute must have a nonnull selector.                                   |
| RBI0019 | Usage    | Error    | The selector string cannot contain any whitespace characters.                        |
