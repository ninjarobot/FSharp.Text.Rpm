namespace fs.rpm

open System

module RpmSpec =

    type License =
        | GPL1
        | GPL2
        | GPL3
        | MIT
        | BSD
        | BSD2Clause
        | BSD3Clause
        | Apache

    type AmusementGroup =
        | Games
        | Graphics

    type ApplicationsGroup =
        | Archiving
        | Communications
        | Databases
        | Editors
        | Emulators
        | Engineering
        | File
        | Internet
        | Multimedia
        | Productivity
        | Publishing
        | System
        | Text

    type DevelopmentGroup =
        | Debuggers
        | Languages
        | Libraries
        | System
        | Tools

    type SystemEnvironmentGroup =
        | Base
        | Daemons
        | Kernel
        | Libraries
        | Shell

    type UserInterfaceGroup =
        | Desktops
        | X
        | XHardwareSupport

    type Group =
        | Amusements of AmusementGroup
        | Applications of ApplicationsGroup
        | Development of DevelopmentGroup
        | Documentation
        | SystemEnvironment of SystemEnvironmentGroup
        | UserInterface of UserInterfaceGroup

    type UserContact = {
        Name : string
        Email : string
    }

    type Preamble = {
        Summary : string
        Name : string
        Version : single
        Release : int
        License : License
        Group : Group
        Source : Uri
        URL : Uri
        Distribution : string option
        Vendor : string option
        Packager : UserContact
    }

    type Script =
        | Script of Lines : string list

    type Specification = {
        Preamble : Preamble
        Prep : Script
        Build : Script
        Install : Script option
        Clean : Script option
        Files : string list
    }