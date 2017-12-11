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
        Description : string option
        Prep : Script
        Build : Script
        Install : Script option
        Clean : Script option
        Files : string list
    }

module Builder =
    open RpmSpec

    let private printGroup (group:Group) =
        Reflection.FSharpValue.GetUnionFields (group, typeof<Group>)
        |> function
        | case, vals when vals.Length > 0 ->
            sprintf "%s/%O" case.Name vals.[0]
        | case, _ ->
            sprintf "%s" case.Name

    let private printPackager (userContact:UserContact) =
        sprintf "%s <%s>" userContact.Name userContact.Email

    let printPreamble preamble =
        [
            yield preamble.Summary |> sprintf "Summary: %s"
            yield preamble.Name |> sprintf "Name: %s"
            yield preamble.Version |> sprintf "Version: %f"
            yield preamble.License |> sprintf "License: %O"
            yield preamble.Release |> sprintf "Release: %d"
            yield preamble.Group |> printGroup |> sprintf "Group: %s"
            yield preamble.Source |> sprintf "Source: %O"
            yield preamble.URL |> sprintf "URL: %O"
            if preamble.Distribution.IsSome then
                yield preamble.Distribution.Value |> sprintf "Distribution: %s"
            if preamble.Vendor.IsSome then
                yield preamble.Vendor.Value |> sprintf "Vendor: %s"
            yield preamble.Packager |> printPackager |> sprintf "Packager: %s"
        ]

    let printSpec (spec:Specification) =
        [
            yield! spec.Preamble |> printPreamble
            if spec.Description.IsSome then
                yield! [ "%description"; spec.Description.Value]
            match spec.Prep with
            | Script lines ->
                yield! [""; "%prep"] @ lines
            match spec.Build with
            | Script lines ->
                yield! [""; "%build"] @ lines
            match spec.Install with
            | Some (Script lines) ->
                yield! [""; "%install"] @ lines
            | None -> ()
            match spec.Clean with
            | Some (Script lines) ->
                yield! [""; "%clean"] @ lines
            | None -> ()
            yield! [""; "%files"] @ spec.Files
        ]
