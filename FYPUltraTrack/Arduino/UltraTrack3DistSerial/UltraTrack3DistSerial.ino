

#include <ESP8266WiFi.h>
#include <WiFiClient.h>

const char* ssid = "Lenovo";
const char* password = "Ultrasound";


WiFiServer server(26);
WiFiClient client;



#define TtrigPin D1
//#define TechoPin D5

//#define AtrigPin D1
//#define BtrigPin D1
//#define CtrigPin D1
#define AechoPin D2
#define BechoPin D3
#define CechoPin D4



long durationA, durationB, durationC;
float distanceA, distanceB, distanceC;

String printA = String("A: ");
String printB = String("B: ");
String printC = String("C: ");
String distanceReading = String("");
char turn = 'A';
char command = 'n';
long lastMillis = 0;
long loops = 0;

void setup() {
	Serial.begin(115200);

	Serial.println("setup");
	pinMode(TtrigPin, OUTPUT);
	//  pinMode(AtrigPin, OUTPUT);
	//  pinMode(BtrigPin, OUTPUT);
	//  pinMode(CtrigPin, OUTPUT);

	//  pinMode(TechoPin, INPUT);
	pinMode(AechoPin, INPUT);
	pinMode(BechoPin, INPUT);
	pinMode(CechoPin, INPUT);

	//WiFi.config(ip, gateway, subnet); 

	WiFi.begin(ssid, password);
	Serial.println("Connecting");

	while (WiFi.status() != WL_CONNECTED) {
		delay(500);
		Serial.print(".");
	}

	Serial.print("Connected to ");
	Serial.println(ssid);

	Serial.print("IP Address: ");
	Serial.println(WiFi.localIP());

	// Start the TCP server
	server.begin();
	//  Serial.println("setup finished");

}

void loop()
{
	// Listen for connecting clients
	client = server.available();
	if (client) {
		Serial.println("Client connected");
		while (client.connected()) {

			//timeCyclesPerLoop();

			Serial.flush();
			//Serial.println("-------------------");

			command = client.read();
			if (command == 'T') {

				if (turn == 'A') {
					distanceA = getDistance(TtrigPin, AechoPin);
					turn = 'B';
					delay(10);
				}
				else if (turn == 'B') {
					distanceB = getDistance(TtrigPin, BechoPin);
					turn = 'C';
					delay(10);
				}
				else if (turn == 'C') {
					distanceC = getDistance(TtrigPin, CechoPin);
					turn = 'A';
					delay(10);
				}

				distanceReading = printDistance(distanceA, distanceB, distanceC);
			Serial.println(command);
        command = 'n';
        client.print(distanceReading);
      client.print('\r');
      client.flush();
      Serial.println(distanceReading);
      }

			

			//yield();
			//  Serial.println("\n");
			delay(10); //pause to let things settle
		}
	}

}

void timeCyclesPerLoop() {
	long currentMillis = millis();
	loops++;

	/* By doing complex math, reading sensors, using the "delay" function,
	   etc you will increase the time required to finish the loop,
	   which will decrease the number of loops per second.
	*/

	if (currentMillis - lastMillis > 1000) {
		Serial.print("Loops last second:");
		Serial.println(loops);

		lastMillis = currentMillis;
		loops = 0;
	}
}


float getDistance(int initPin, int echoPin) {
	digitalWrite(initPin, LOW);
	delayMicroseconds(2);
	digitalWrite(initPin, HIGH);
	delayMicroseconds(10);
	digitalWrite(initPin, LOW);
	unsigned long pulseTime = pulseIn(echoPin, HIGH);
	float distance = pulseTime * 0.034;
	return distance;

}


String printDistance(float dist1, float dist2, float dist3) {

	String output = "|D;";
	output += dist1;
	output += ";";
	output += dist2;
	output += ";";
	output += dist3;

	return output;


}



