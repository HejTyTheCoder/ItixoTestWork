# ItixoTestWork
Konzolová .NET aplikace pro automatizované stahování dat z meteostanice, jejich převod do JSON formátu a ukládání do SQL databáze. Program každou hodinu stáhne XML data z konfigurovatelné URL, uloží je spolu s informací o datu a času stažení a vypíše informace do konzole. V případě, že je meteostanice nedostupná, uloží o této situaci informativní záznam.

## Postup spuštění
Aplikace je multiplatformní — spustitelná na Windows, Linux i macOS.  
V kořenovém adresáři projektu najdete skripty pro spuštění:
- `run-windows.bat`
- `run-linux.sh`
- `run-macos.sh`

Na Linuxu a macOS je po prvním stažení repozitáře potřeba nastavit spustitelné oprávnění pro spouštěcí skripty:
- `chmod +x run-linux.sh`
- `chmod +x run-macos.sh`

Poté lze skripty spustit přímo příkazem `./run-linux.sh` nebo `./run-macos.sh`.

Pro běh není nutné mít nainstalovaný .NET runtime — vše potřebné je součástí publikovaných verzí.

## Informace o uložených datech a logování
Aplikace automaticky v kořenovém adresáři projektu vytváří složku db/, ve které se nachází SQLite databáze se staženými daty.
Stejně tak je vytvořena i složka logs/ pro logovací soubory s podrobným záznamem běhu aplikace.