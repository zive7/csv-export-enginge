# CSV export enginge

A library that can help you easily generate CSV files. It supports custom rounding, custom header and content translation, options to restructure the content of the file according to your provided property list and optionally to hide the header. Additionally, there is a demo console app that can show you how to use it.


# How to validate a CSV file?
You just need to provide the columns list that is part of your configuration map class. It checks if the map class exists and validates the provided property columns. When the processing is done it's going to return a result object with some info.

# How to generate a CSV file?
Create the configuration map class and then get the data from somewhere and call some of the 'generate' methods from the export service class. This service is going to return an array of bytes which you can store somewhere.

To be continued...
