# FileTypeIdentifier

`FileTypeIdentifier` is a simple library that helps identifying file types by using a user-configurable collection of [magic numbers](https://en.wikipedia.org/wiki/File_format#Magic_number).

This library is not designed with any black-magic, super-efficient file type checking tricks in mind - it identifies a file by matching the file's [magic number](https://en.wikipedia.org/wiki/File_format#Magic_number) with a user-configured collection of magic numbers.

Found inside this repo is a [limited set of pairs of magic numbers and file names](./JNysteen.FileTypeIdentifier/MagicNumbers) (based on my current needs use cases for identifying files). 
If you think that the set of preconfigured magic numbers is too limited, you are more than welcome to create a pull request or contact me with suggestions.

## Features

**What the `FileTypeIdentifier` can do:**
* Look at the initial bytes of a file and determine whether the file's magic number matches a known file type or not.

**What the `FileTypeIdentifier` *cannot* do:**
* Guarantee that the file is actually of the type indicated by its magic number.
* Ensure that the file is not corrupt or malicious.

## Use cases

I am personally using this project for...:

* ... quickly determining whether a file is a type of file my application can handle or not.
* ... determining the type of files received in web services, where the extension of the file might not match the actual file type.
* ... deciding how to process files based on their actual file type.


## Example usage

 [A small console application](./Samples/JNysteen.FileTypeIdentifier.ConsoleApplication) using the file type identifier can be found in [the samples folder](./Samples).
