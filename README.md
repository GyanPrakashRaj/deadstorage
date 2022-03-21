# deadstorage

My purpose is to use this tool as a password manager with [KeePassXC](https://keepassxc.org/),
and also INTERLOCK (file encryption front end).

### 1. Preparing your own microSD card
- 1. check microSD-compatibility
- 2. [burn] Void Linux image into microSD card 

I chose a Samsung microSD and installed Void Linux as described in their wiki.
I choose Void Linux, but there are other available image.
### 2. Connect to USB killer

#### Option 1 - serial
We can use USB to TTL cable to connect to USB killer serial port.
I solder a header in USB killer and use pins 1,5,6 to connect a `USB to TTL` adapter with silicon CP210x chipset and specific [drivers](https://www.silabs.com/products/development-tools/software/usb-to-uart-bridge-vcp-drivers).
To connect in macOS use the next command:
```
screen /dev/tty.SLAB_USBtoUART 115200
```

#### Option 2 - ssh connection
The Void Linux image comes with predefined ipv4 address `10.0.0.1`, so set your IP address to `10.0.0.2` and connect to your USB killer. 

Now you can log in with
```
ssh 10.0.0.1 -l killer
```

### 3. Prepare KeePass
Setup is done with Ansible
  - setup ssh
  - install keepassxc, etckeeper and standard utilities

### Notes
We need the `RNDIS/Ethernet Gadget` interface in the macOS Network Preferences.

To share macOS internet access with the USB killer device, you can:

option 1
```
sudo ./scripts/connection_share.sh
```

option 2
  - set USB killer IP to `192.168.2.X/24` and gateway `192.168.2.1`
  - in macOS set the IP address of `RNDIS/Ethernet Gadget` interface to `192.168.2.1`
  - finally, set `Enable Internet Sharing` to ON in System Preferences

To share Linux internet connection with the USB killer device, just run:
```
sudo ./scripts/linux_connection_share.sh
```

