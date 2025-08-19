# Mini Shopping Cart (Risk-Based Testing with Mind Maps)

This project demonstrates how **mind mapping** can support **Risk-Based Testing (RBT)** in a small web application.  
It was built for SOEN 6971 (Summer 2025) at Concordia University.

## ✨ Features
- Blazor WebAssembly shopping cart
- Add/remove products, dynamic totals
- Prevents invalid actions (e.g., unavailable products, negative totals)
- Automated tests with **xUnit + bUnit**
- Risk analysis supported by **mind maps and software metrics**

## 📂 Repository Structure
- `/MiniCart` – Source code for the shopping cart  
- `/MiniCart.Tests` – Automated tests (xUnit + bUnit)  
- `/docs` – Risk matrix, mind maps, and report figures  

## 🚀 Getting Started
### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- Visual Studio 2022 (recommended) or VS Code

### Run the App
```bash
git clone https://github.com/priscila-aramouni/shopping-cart.git
cd shopping-cart/MiniCart
dotnet run
