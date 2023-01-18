***Admin Application***

This Web-Application was created for Project C at Hogeschool Rotterdam. It's built in ASP.NET (C#) following the Model-View-Controller architecture.
The User interface consists of CSHTML files, a file extension by Microsoft which allows C# code to be used alongside HTML and CSS code.
For Logic, C# is used alongside Entity-FrameworkCore, an ORM by Microsoft that is built on the use of C# Object classes as tables in the Database.
This Readme file should be able to explain every aspect of this application. 

GitHub: https://github.com/Stephanos2911/AdminApplication

**WebServer**

To Visit this Application : 145.24.222.31:8500

The latest version of this application is running on a Virtual Windows Machine from Hogeschool Rotterdam, using IIS. 
To enter this virtual machine, use Remote Destkop Connection built in to windows. 
Connection: 145.24.222.31:8100
Login using Username: win-1033554 and Password: X%243Hp66KQ


**Database**

Login to The MySQL server with this account:
Username: admin
Password: AdminGroep62022

Both this application and the UserApplication use the same MySQL database that is being run on the same Virtual Machine (using MySQL Server). 
To use EntityFramework with an MySQL server (default EntityFramework only supports SQLServer), EntityFramework is used with an extension to support MySQL. 
It's installed using the NuGet installer inside of Visual Studio. At Runtime, a EntityFramework establishes a connection to the database using the 
ConnectionString found at the top of AppSettings.Json. Interaction with the database is done using the _context object found in every controller's constructor (except AccessController, 
which is only used for logging in). All database interactions such as CRUD and Queries are performed with the _context.

**Architecture**

This application follows MVC, thus every component is found under it's corresponding folder (using name conventions). 
The Product, Store, Employee and Message controller are all built on similar code. Every IActionResult/ViewResult method calls 
it's corresponding CSHTML file in the Views folder (folder name is the same as controller name), or can be redirected to an other view
using the RedirectToAction function. 

**GET and POST**

I'm now going to use the process of adding a product to the database to explain the use of GET/POST and Entity-Framework.

The [HTTPGET] tag above a method (example: AddProduct) clarify the use of the HTTP-GET functionality provided by ASP.NET MVC. 
When a user presses the "Add Product" button found on the ProductIndex page, the AddProduct[HTPPGET] method inside the ProductController gets called.
This method creates an Empty ProductViewModel class (basically a normal Product class but with some different properties and built in Form-Validation)
and sends it to AddProduct.CSHTML to be used in a form. The user fills out the form, adding data to the object. When the user presses Submit, this same ProductViewModel object 
gets sent to the AddProduct[HTPPPOST] method inside of ProductController. Here an empty Product.cs object is created and populated with the data inside of the ProductViewModel.

Our application now has a valid Product.cs object to be written into our database. We can do this using the _context.Add(NewProductObject) function.
The only thing then left to do is the use of the _context.SaveChanges() function, which actually performs the SQL command to our database. This function is called at the end of 
every Add,Delete or edit function, without it no data is written.



