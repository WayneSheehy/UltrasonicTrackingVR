/*
 Name:		UltraTrackController.ino
 Created:	12/4/2018 4:44:30 PM
 Author:	Wayne
*/

#include <ESP8266WiFi.h>
#include <WiFiClient.h>
#include <MPU9250_asukiaaa.h>


const char* ssid = "Lenovo";
const char* password = "Ultrasound";
//IPAddress ip(192, 168, 0, 178);  
//IPAddress gateway(192, 168, 0, 1);  
//IPAddress subnet(255, 255, 255, 0);  
WiFiServer server(27);
WiFiClient client;

#define echoPin D5 // Echo Pin connected to sensor
#define trigPin D6 // Trigger Pin connected to sensor
#define gunTrigger D3
#define greenLED D7
long duration, distance; // Used to calculate distance
String dataToSend = "";

#define AHRS true         // Set to false for basic data read
#define SerialDebug true  // Set to true to get Serial output for debugging

MPU9250 mySensor;
float gX, gY, gZ;
int buttonState = 0;
int fire = 0;
char command = 'n';
int counter = 0;

void setup() {
	Serial.begin(115200);
	Wire.begin();
	mySensor.setWire(&Wire);
	mySensor.beginGyro();
	// Set our pins
	pinMode(trigPin, OUTPUT);
	pinMode(echoPin, INPUT);
	pinMode(greenLED, OUTPUT);
	pinMode(gunTrigger, INPUT);

	digitalWrite(greenLED, LOW);

	//WiFi.config(ip, gateway, subnet); 
	WiFi.begin(ssid, password);
	Serial.println("Connecting");

	while (WiFi.status() != WL_CONNECTED) {
		digitalWrite(greenLED, HIGH);
		delay(1000);
		digitalWrite(greenLED, LOW);
		Serial.print(".");
	}

	digitalWrite(greenLED, HIGH);
	Serial.print("Connected to ");
	Serial.println(ssid);

	Serial.print("IP Address: ");
	Serial.println(WiFi.localIP());

	// Start the TCP server
	server.begin();
}

void loop() {
	// Listen for connecting clients
	client = server.available();
	if (client) {
		Serial.println("Client connected");
		while (client.connected()) {
			Serial.flush();

			command = client.read();
			
				
			
//			 if (counter <= 0)
//			{
//				mySensor.gyroUpdate();
//				gX = mySensor.gyroX();
//				gY = mySensor.gyroY();
//				gZ = mySensor.gyroZ();
//dataToSend += printGyroOutput();
//				counter = 10;
//			}
//			counter -= 1;
			buttonState = digitalRead(gunTrigger);

			// check if the pushbutton is pressed. If it is, the buttonState is HIGH:
			if (buttonState == HIGH) {

				fire = 1;
			}
			else {

				fire = 0;
			}


			dataToSend += "|F;";
      dataToSend += fire;
			// Send the distance to the client, along with a break to separate our messages
			client.print(dataToSend);

			client.print('\r');
			client.flush();
			Serial.println(dataToSend);
dataToSend = "";
			// Delay before the next reading
			delay(10);
		}
	}
}

String printGyroOutput() {

	String output = "|G;";

	output += gX;
	output += ";";
	output += gY;
	output += ";";
	output += gZ;
	

	return output;
}


