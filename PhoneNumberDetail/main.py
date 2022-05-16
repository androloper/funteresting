import phonenumbers
from phonenumbers import timezone, geocoder, carrier

phoneNumber = phonenumbers.parse('+905060526793')
timezone = timezone.time_zones_for_number(phoneNumber)
carrier = carrier.name_for_number(phoneNumber, 'tr')
region = geocoder.description_for_number(phoneNumber, 'tr')
print(phoneNumber) #Country Code: 90 National Number: 5060526793
print(timezone) #('Europe/Istanbul',)
print(carrier) #Turk Telekom
print(region) #Türkiye