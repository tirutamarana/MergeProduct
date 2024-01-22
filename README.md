**Instructions:**

Clone the project from **master** branch and follow below steps:
  1. Make sure to have .net core 3.1 sdk is installed.
  2.     
    
    Console app dependencies:
        CsvHelper Version=19.0.0
        Newtonsoft.Json Version=9.0.1

    NUnit test project dependencies:
        NUnit Version=3.12.0
        NUnit3TestAdapter Version=3.16.1
        Microsoft.NET.Test.Sdk Version="16.5.0
  3. Go to properties and set copy always for all the highlighted files.
  4. Setting: ![Copy always](https://github.com/tirutamarana/MergeProduct/assets/157348330/797d0993-af5c-4cee-a197-1ef2c520f820)

  5. Files: ![Data Files](https://github.com/tirutamarana/MergeProduct/assets/157348330/861ba0fc-e2d3-4a47-b888-e9873fda14c6)
  6. DataConfig.json: This file will have the filenames and their paths.
  7. Build the solution and make sure to restore nuget packages.
  8. Execution: Two ways to see the results.
     
                1. Build the test project and run all tests. When all tests are passed, check the test project bin folder for \bin\Debug\output\result_output.csv
                2. Run the console application. When it is finished, check the console app bin folder for \bin\Debug\netcoreapp3.1\output\result_output.csv
  9. Improvements:
      
            1. Handling Exception and logging the errors.
            2. NUnit test project is on .net core 3.1. It can be use upgraded to supported version .net 6.0+.


