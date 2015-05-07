#include <Wire.h>

#define CTRL_REG1 0x20
#define CTRL_REG2 0x21
#define CTRL_REG3 0x22
#define CTRL_REG4 0x23

int Addr = 105;                 // direccion I2C del giroscopio
int x, y, z;

void setup(){
  Wire.begin();
  Serial.begin(9600, SERIAL_8E1);
  writeI2C(CTRL_REG1, 0x1F);    // Enciende todos los ejes
  writeI2C(CTRL_REG3, 0x08);    // Activa señal de control
  writeI2C(CTRL_REG4, 0x80);    //  Selecciona escala de 500° por segundo
  delay(100);                   //  Espera la sincronizacion
}

void loop(){
  getGyroValues();              // Obtiene valores nuevos
  // Dividirlo entre 114 reduce el ruido (puede ser cualquier numero)
  Serial.print(x/114);
  Serial.print(" ");
  Serial.print(y/114);
  Serial.print(" ");
  Serial.println(z/114);
  delay(125);  // Pequeño delay entre lecturas
}

void getGyroValues () {
  byte MSB, LSB;

  MSB = readI2C(0x29);
  LSB = readI2C(0x28);
  x = ((MSB << 8) | LSB);

  MSB = readI2C(0x2B);
  LSB = readI2C(0x2A);
  y = ((MSB << 8) | LSB);

  MSB = readI2C(0x2D);
  LSB = readI2C(0x2C);
  z = ((MSB << 8) | LSB);
}

int readI2C (byte regAddr) {
    Wire.beginTransmission(Addr);
    Wire.write(regAddr);                // Direccion de registro para leer
    Wire.endTransmission();             // Termina la peticion
    Wire.requestFrom(Addr, 1);          // Lee el byte
    while(!Wire.available()) { };       // Espera la recepcion
    return(Wire.read());                // Obtiene resultado
}

void writeI2C (byte regAddr, byte val) {
    Wire.beginTransmission(Addr);
    Wire.write(regAddr);
    Wire.write(val);
    Wire.endTransmission();
}
