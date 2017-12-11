(*
 * Example script to make a simple RPM spec.
 *)

#r "bin/Debug/net462/fs.rpm.dll"

open fs.rpm
open fs.rpm.RpmSpec

let spec = {
    Preamble = 
        {
        Summary = "testing rpm spec builder"
        Name = "testrpm"
        Version = 1.0 |> single
        Release = 1
        Group = Development(DevelopmentGroup.Libraries)
        Source = "http://artifactory/test.tgz" |> System.Uri
        URL = "http://readthedocs.io/test" |> System.Uri
        Distribution = "RedHat" |> Some
        Vendor = "testerama" |> Some
        Packager =
            {
                Name = "Dave"
                Email = "dave@example.net"
            }
        License = MIT
    }
    Description = "this is just a test spec, enjoy!" |> Some
    Prep = Script ["%setup"]
    Build = Script ["make"]
    Install = None
    Clean = None
    Files = ["/tmp/rpmbuild/x.so"; "/tmp/rpmbuild/y.so"]
}

spec |> Builder.printSpec |> List.iter (printfn "%s")