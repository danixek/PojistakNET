# 🧾 Globální TO-DO – Pojišťák.NET

# 🧾 Pojišťák.NET – Evidence pojištění v ASP.NET

> **Status:** Stabilní verze s možností dalšího rozšiřování  
> **Tech Stack:** ASP.NET Core MVC, EF Core, ASP.NET Identity, MSSQL

---

## 🧩 O projektu

**Pojišťák.NET** je webová aplikace pro správu pojištěnců a jejich smluv, vytvořená jako součást rekvalifikačního studia.  
Projekt demonstruje práci s:

- autentizací a správou rolí (Identity)
- relacemi mezi entitami (pojištěnec ↔ pojištění)
- oddělením veřejné a privátní části aplikace
- správou přístupových práv

Cílem bylo vytvořit funkční, přehlednou a rozšiřitelnou aplikaci v ASP.NET Core bez závislosti na externích službách.

---

## ✅ Hotové funkce

- Registrace a přihlášení uživatelů (ASP.NET Identity)
- Role: `User`, `Admin`, `SuperAdmin`
- Přesměrování na dashboard dle role
- Oddělení veřejné části od přihlášené
- Admin CRUD rozhraní pro správu pojištěnců a jejich pojištění
- Logging a auditní záznamy (např. přihlášení, změny)
- Validace formulářů
- Dependency Injection, konfigurace `DbContext`
- Základní testování (ruční + scénáře CRUD)

---

## 🟡 Možnosti rozšíření (neblokující)

Projekt je ve stabilní fázi, další funkce lze doplnit v budoucnu:

- Paging při větším objemu dat (tisíce záznamů)
- Vylepšená ochrana proti XSS
- Uživatelská editace vlastního profilu
- CI/CD pipeline
- Obnova/změna hesla, dvoufázové ověření
- UI/UX vylepšení (Tailwind, responzivita, spinner, 404)
- Oprava Helperů pro lepší UX

---

## 🛠️ Ukázka (volitelné)

> Sem můžeš vložit screenshoty nebo GIF ukázek z UI, případně demo link.

---

## 🧠 Shrnutí

- Projekt vznikl jako závěrečný projekt při rekvalifikačním kurzu od ITnetwork.
- Plní náborové očekávání pro juniorní roli – základní funkce, role, bezpečnost, čistý kód.
- Architektura umožňuje další rozvoj bez zásahu do základů.

---

> 💡 **Poznámka:** Projekt byl refaktorován kvůli technickým omezením a prioritám rekvalifikačního úkolu. Aktuální verze je plně funkční a vhodná pro portfolio.
