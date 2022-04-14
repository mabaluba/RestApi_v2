# KlyukvinM

## Structure

### Each module should be exist in the `./src/*` folder.
~~~
./src
    |> M01 <-- folder
        |> M01.sln
        |> {Project} <-- folder
            |> {Project}.csproj
    |> M02 <-- folder
        |> M02.sln
        |> {Project} <-- folder
            |> {Project}.csproj
        |> {Project}.Tests <-- folder
            |> {Project}.Tests.csproj
~~~

### Tests projects should exist near the target project, with name: `<TargetProject>.Tests`

### Use latest framework, currently it is the `net5.0`
