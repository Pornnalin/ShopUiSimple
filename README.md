# ShopUiSimple
## Project Overview
This Unity script manages the shop UI, including item display, sorting, and filtering.

### Problems and Solutions
#### 1. Problem: Displaying Static Data at Runtime
- **Issue:** Need to display information (name, price, image) that remains constant at runtime and can be shared with other objects
- **Solution:**  Use ScriptableObject to store data, which can hold string, int, and sprite values that remain unchanged

#### 2. Problem: Sorting Items
- **Issue:** Need to sort items in ascending order (A-Z, Z-A), by price (low to high, high to low), and update the item order in the Game View
- **Solution:**  Use List.Sort() for sorting based on the selected method, and then use SetSiblingIndex to update the order in the Hierarchy

#### 3. Problem: Filtering Items by Category
- **Issue:** Need to toggle visibility of items based on selected categories
- **Solution:** Check categories from ScriptableObject and use Toggles to manage visibility based on the selected category

