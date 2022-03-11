# deadstorage

My purpose is to use this tool as a password manager with [KeePassXC](https://keepassxc.org/),
and also [INTERLOCK](https://github.com/f-secure-foundry/interlock) (file encryption front end).

### 1. Preparing your own microSD card
- 1. check [microSD-compatibility](https://github.com/inversepath/usbarmory/wiki/microSD-compatibility)
- 2. [burn](https://voidlinux.miraheze.org/wiki/USB_Armory#Installation) Void Linux image into microSD card 

I chose a Samsung microSD and installed Void Linux as described in their [wiki](https://voidlinux.miraheze.org/wiki/USB_Armory).
I choose Void Linux, but there are other [available images](https://github.com/inversepath/usbarmory/wiki/Available-images) for USB armory.

### 2. Connect to USB armory

#### Option 1 - serial
We can use USB to TTL cable to connect to USB armory serial port; the breakout header is described in [gpio page](https://github.com/inversepath/usbarmory/wiki/GPIOs).

I solder a header in USB armory and use pins 1,5,6 to connect a `USB to TTL` adapter with silicon CP210x chipset and specific [drivers](https://www.silabs.com/products/development-tools/software/usb-to-uart-bridge-vcp-drivers).
To connect in macOS use the next command:
```
screen /dev/tty.SLAB_USBtoUART 115200
```

#### Option 2 - ssh connection
The Void Linux image comes with predefined ipv4 address `10.0.0.1`, so set your IP address to `10.0.0.2` and connect to your USB armory. 

Now you can log in with
```
ssh 10.0.0.1 -l usbarmory
```

### 3. Prepare KeePass
Setup is done with Ansible
  - setup ssh
  - install keepassxc, etckeeper and standard utilities
