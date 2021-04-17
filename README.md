# amazon-scalper
simple and fast amazon scalper made in C#

# setup
firstly open up App.config and drop all of your amazon info into there,
by default it is linked to a 3080 page

you will also need to download chromedriver.exe for the whole thing to work you can get that here:
https://chromedriver.storage.googleapis.com/index.html?path=90.0.4430.24/

link the directory that the chromedriver.exe is stored in inside App.config aswell

run the app :)

# testing
to see that it works firstly uncomment line 84 in Program.cs otherwise you'll have to cancel your order...
also enter this link in the url section of App.config:
https://www.amazon.ca/stores/page/6944F919-5022-4BD5-8E4C-1D67AA822C3B
it should try to checkout a laptop
