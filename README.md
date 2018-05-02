# sms-portal
a full portal containing web panel, OS services, management core, GSM cluster modem interface and ... to connect to and manage a clusterd GSM modem using duplex channel sockets.

Project includes following sub-projects:
1) ServerUI: a full windows form application to handle all functionalities of gsm modem including send and receive massive sms, delivery report, USSD commands and ...
2) GSM Handler: Another windows app wich is responsible to handle each process connected to each module on GSM modem
3) GSM Connector: The main connector to each module on GSM modem. 
4) SMSProtalService: a WCF web service which access two functionality on web including AddToQueue and ProccessScheduleQueue to add new sms set to send and managing a scheduled sending respectively.
5) SMS Portal Web Panel: a ASP.Net MVC project to handle sort of functionalities such as managing persons, organizations, authentications and authorization, contancs, sms inbox and outbox, visual reports and ...

# hardware:
the hardware of project is a clustered GSM modem made by 2 or 3 SIM800L modules connectes to an ARM M3 microcontroller on each modules through UART. the processor also handles a RJ45 ethernet module on SPI to connect to software counterpart
