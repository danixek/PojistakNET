# 🧾 Pojišťák.NET – Evidence pojištění v ASP.NET

Tento projekt vznikl jako závěrečný úkol rekvalifikačního kurzu u ITnetwork.

Původní verze mi přišla funkční, ale koncepčně nedotažená – proto jsem se pustil do její revize a rozšíření,  
aby lépe reprezentovala mé dovednosti a mohla sloužit jako ukázka pro hledání juniorní pozice v IT.

---

## 🧩 O projektu

**Pojišťák.NET** je jednoduchá webová aplikace pro evidenci pojištěnců a jejich smluv.  
Postaveno na ASP.NET Core MVC, využívá ASP.NET Identity pro správu uživatelů a Entity Framework Core pro databázovou vrstvu.

Aplikace demonstruje práci s autentizací, správou rolí, relacemi mezi entitami a oddělením veřejné/přihlášené části.

---

## ✅ Stav projektu

Projekt je **považován za dokončený** a momentálně **není v aktivním vývoji**.

Základní funkcionalita je plně implementována a odpovídá běžnému očekávání pro juniorní roli v oblasti .NET vývoje.  
Přesto existují oblasti, které by bylo možné dále rozšířit nebo zlepšit – například:

- ochrana proti XSS útokům,
- editace uživatelského profilu běžným uživatelem,
- CI/CD pipeline a auditní logování.

Tyto prvky nebyly implementovány z důvodu prioritizace stability, času a technických omezení.  
Projekt však zůstává otevřený dalšímu vývoji v budoucnu, pokud k tomu bude motivace nebo potřeba.

---

## ✨ Implementované funkce

- ASP.NET Identity – registrace, přihlášení, správa rolí (`User`, `Admin`, `SuperAdmin`)
- Tvorba článků a správa účtů, včetně adminů.
- Základní dashboard a rozlišení přístupových práv
- CRUD operace pro správu pojištěnců a jejich pojištění
- Logging a auditní záznamy (přihlášení, změny dat)
- Validace formulářů
- Oddělení veřejné a zabezpečené části aplikace
- DI, registrace `DbContext`, základní testování

---

## 🟡 Potenciál pro rozšíření (neblokující)

- Paging pro větší objemy dat  
- XSS ochrana mimo vestavěné mechanismy  
- Uživatelská editace vlastních údajů  
- CI/CD, 2FA, responsivita, spinner, error stránky
- oprava Helperů pro lepší UX

---

## 🧠 Shrnutí

Projekt slouží jako ukázka praktických schopností ve vývoji webových aplikací v ASP.NET Core.  
Splňuje náborová očekávání na úrovni juniorního vývojáře a je připravený k prezentaci v portfoliu.

> 🎯 Cílem nebyla dokonalost, ale funkční, čitelný a rozšiřitelný projekt, který ukazuje reálné dovednosti – ne jen splnění šablonovitého zadání.

## Jak projekt spustit

Pro spuštění projektu doporučuji použít pokročilé editory jako Visual Studio Community nebo JetBrains Rider.
Alternativně lze použít i Visual Studio Code s doinstalovaným rozšířením C# Dev Kit, který nainstaluje .NET SDK včetně nástroje dotnet.

1. Naklonujte repozitář  
   `git clone https://github.com/danixek/PojistakNET.git`  
   `cd PojistakNET`
2. Ověřte připojení k databázi v souboru `appsettings.json`  
   (pokud používáte výchozí nastavení, přeskočte)
3. Sestavte projekt:  
   `dotnet build`  
   Spuštěním se zkontroluje struktura projektu a automaticky se stáhnou závislosti - NuGet balíčky.
4. Proveďte migraci databáze:
   ```bash příkazy  
   dotnet ef database update --context ApplicationDbContext  
   dotnet ef database update --context InsuranceContext
5. Spusťte projekt:  
   `dotnet run`
   
> 💡 **Poznámka:** Pokud se příkaz `dotnet ef` nezdaří, je pravděpodobně potřeba doinstalovat EF CLI nástroj:  
`dotnet tool install --global dotnet-ef`

Po úspěšném spuštění se v konzoli objeví adresa (např. https://localhost:5001).
Otevřete ji v prohlížeči – projekt by měl být dostupný.
Ve Visual Studiu Community nebo Rideru se aplikace často spustí automaticky s otevřením prohlížeče.

