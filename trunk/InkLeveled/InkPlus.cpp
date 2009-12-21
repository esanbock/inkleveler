#include <string>
#include <vector>
#include <iostream>

#include <inklevel.h>
#include "leveler.h"

using namespace std;

int main( int argc, char** argv  )
{
	int deviceNum = 0;
	
	if( argc != 2 )
	{
		cout << "InkPlus <device number>" << endl;
		return -1;
	}

	const char* deviceFile = argv[1];
	
	try
	{
		cout << "Retrieving information about USB lp..." << deviceFile << endl;
		Printer firstPrinter = getusbprinterlevels(deviceFile);
		cout << "Found " << firstPrinter.Name() << endl;
	}
	catch( int e )
	{
		cout << "Error " << e << endl;
		return e;
	}
	
	return 0;
}
