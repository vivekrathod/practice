#include <iostream>
#include <sstream>
#include "windows.h"	
#include "sqlext.h"
using namespace std;

void ConnectUsingConnectionString(SQLHDBC hdbc1);
void ConnectUsingDSN(SQLHDBC hdbc1);
string GetErrors(SQLSMALLINT htype, SQLHANDLE hndl);

int main()
{
	cout << "Testing ODBC..." << endl;
	
	SQLHENV henv1;
	if (!(SQL_SUCCEEDED(SQLAllocHandle(SQL_HANDLE_ENV, SQL_NULL_HANDLE, &henv1))))
	{
		cout << "Errors allocating env hanlde: " << GetErrors(SQL_HANDLE_ENV, henv1) << endl;
		return -1;
	}
		

	if (!(SQL_SUCCEEDED(SQLSetEnvAttr(henv1, SQL_ATTR_ODBC_VERSION, (LPVOID)SQL_OV_ODBC3, SQL_IS_INTEGER))))
	{
		cout << "Error setting environment: " << GetErrors(SQL_HANDLE_ENV, henv1) << endl;
		return -1;
	}
		

	SQLHDBC hdbc1;
	if (!(SQL_SUCCEEDED(SQLAllocHandle(SQL_HANDLE_DBC, henv1, &hdbc1))))
	{
		cout << "Errors allocating connect hanlde: " << GetErrors(SQL_HANDLE_DBC, hdbc1) << endl;
		return -1;
	}
		

	cout << "Connecting using connection string" << endl;
	ConnectUsingConnectionString(hdbc1);

	cout << "Connecting using DSN " << endl;
	ConnectUsingDSN(hdbc1);

}

void ConnectUsingConnectionString(SQLHDBC hdbc1)
{
	string connString = "DRIVER={IBM DB2 ODBC DRIVER};Database=sample;hostname=chamco;port=50000;protocol=TCPIP;uid=administrator;pwd=Admin123";
	char connStringOut[200]; int actualConnStringOut;
	if (SQL_SUCCEEDED(SQLDriverConnectA(hdbc1, NULL, (SQLCHAR*)connString.c_str(), connString.size(), (SQLCHAR*)connStringOut, sizeof(connStringOut), (SQLSMALLINT*)&actualConnStringOut, SQL_DRIVER_NOPROMPT)))
	{
		cout << "Connected successfully!" << endl;
	}
	else
	{
		cout << "Error(s) connecting: " << GetErrors(SQL_HANDLE_DBC, hdbc1) << endl;
	}
}

void ConnectUsingDSN(SQLHDBC hdbc1)
{
	// this requires a DSN to be setup
	string dsn = "dsn1";
	string user = "admininstrator";
	string password = "Admin123";
	if (SQL_SUCCEEDED(SQLConnectA(hdbc1,
	(SQLCHAR*)dsn.c_str(),
	dsn.size(),
	(SQLCHAR*)user.c_str(),
	user.size(),
	(SQLCHAR*)password.c_str(),
	password.size())))
	{
		cout << "Connected successfully!" << endl;
	} 
	else
	{
		cout << "Error(s) connecting: " << GetErrors(SQL_HANDLE_DBC, hdbc1) << endl;
	}
}

string GetErrors(SQLSMALLINT htype, SQLHANDLE hndl)
{
	stringstream errors;
	SQLCHAR message[SQL_MAX_MESSAGE_LENGTH + 1];
	SQLCHAR sqlstate[5/*SQL_SQLSTATE_SIZE*/ + 1];
	SQLINTEGER sqlcode;
	SQLSMALLINT length, i = 1;
	// Get diagnostic record(s)
	while (SQLGetDiagRecA(htype,
		hndl,
		i,
		sqlstate,
		&sqlcode,
		message,
		SQL_MAX_MESSAGE_LENGTH + 1,
		&length) == SQL_SUCCESS)
	{
		errors << "Diagnostic record: " << i << ", SQLSTATE: " << sqlstate << ", Native SQL Error Code: " << sqlcode << ", Error message: " << message << endl;
		i++;
	}

	return errors.str();
}