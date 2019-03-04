
Readme:

1. All acceptance tests are written in c# using VSTS tool, specflow and NUnit libraries.
2. Initialization is done through Background tag which creates the http request and saves the response locally.
3. Acceptance Test 1 : Checks if response status code is 200.
4. Acceptance Test 2 : Checks if Category Name is Carbon credits in the response
5. Acceptance Test 3 : Checks if CanRelist flag is True in the response
6. Acceptance Test 4 : Checks if Promotion element is 'Gallery' with description containing text as '2x larger image'.

Highlights of the Application:

1. Configurable: By using BDD features.
2. Modular : The responsibility has been divided between different classes.
3. Maintainable : Any new acceptance test can be easily added and tested
4. Exception handling : Asserts has beed added. If test fails, it reports and control comes out of the application.


Future Enhancements:

1. Should execute multiple requests which can be passed from the Test Data file.
2. Save the response in the application folder for debugging purpose.
