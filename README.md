# FileTypeIdentifier

`FileTypeIdentifier` is a simple component that can identify files types using a user-configurable collection of files' magic numbers..

This component is not designed with any black-magic, super-efficient file type checking tricks in mind - it identifies a file by matching the file's [magic number](https://en.wikipedia.org/wiki/File_format#Magic_number) with a user-configured collection of magic numbers.

Found inside this component is a [very limited set of pairs of magic numbers and file names](./JNysteen.FileTypeIdentifier/MagicNumbers) (based on my current needs use cases for identifying files). 
If you feel like the set of preconfigured magic numbers is too limited, you are more than welcome to contact me with suggestions.

## Features - spelled out

**What the `FileTypeIdentifier` can do:**
* Look at the header of a file and tell whether the file's magic number matches a known type or not.

**What the `FileTypeIdentifier` *cannot* do:**
* Guarantee that the file is actually of the type indicated by its magic number.
* Ensure that the file is not corrupt.


## Use cases

I will personally be using this project when...:

* ... quickly determining whether a file is a type of file my application can handle or not.
* ... determining the type of files received in web services, where the extension of the file might not match the actual file type.
* ... deciding how to process files based on their actual file type.


## How to use

 [A small console application](./Samples/JNysteen.FileTypeIdentifier.ConsoleApplication) using the file type identifier can be found in [the samples folder](./Samples).
