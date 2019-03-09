# FileTypeIdentifier

`FileTypeIdentifier` is a simple component that can match files with a user-configurable collection of file signatures.

This component is not designed with any black-magic, super-efficient file type checking tricks in mind - it identifies a file by matching the file's signature with a user-configured collection of file signatures.

Found inside this component is a very limited set of pairs of file signatures and file names (based on my current needs use cases for identifying files). 
If you feel like the set of preconfigured file signatures is too limited, you are more than welcome to contact me with suggestions for more signatures.

## Use cases

I will personally be using this project when...:

* ... quickly determining whether a file is a type of file my application can handle or not.
* ... determining the type of files received in web services, where the extension of the file might not match the actual file type.
* ... deciding how to process files based on their actual file type.


## How to use

 [A small console application](.\Samples\JNysteen.FileTypeIdentifier.ConsoleApplication) using the file type identifier can be found in [the samples folder](.\Samples\).