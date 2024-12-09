const apiUrl = "https://localhost:7000/api/contacts";

async function fetchContacts() {
    const response = await fetch(apiUrl);
    const contacts = await response.json();
    const tableBody = document.querySelector("#contacts-table tbody");
    tableBody.innerHTML = "";
    contacts.forEach(contact => {
        const row = document.createElement("tr");
        row.innerHTML = `
            <td>${contact.id}</td>
            <td>${contact.firstName}</td>
            <td>${contact.lastName}</td>
            <td>${contact.email}</td>
            <td>${contact.phoneNumber || ""}</td>
            <td>${contact.address || ""}</td>
            <td>
                <button class="btn btn-sm btn-warning" onclick="editContact(${contact.id})">Edit</button>
                <button class="btn btn-sm btn-danger" onclick="deleteContact(${contact.id})">Delete</button>
            </td>
        `;
        tableBody.appendChild(row);
    });
}

async function addOrUpdateContact(event) {
    event.preventDefault();
    const contactId = document.querySelector("#contact-id").value;
    const method = contactId ? "PUT" : "POST";
    const url = contactId ? `${apiUrl}/${contactId}` : apiUrl;
    const contact = {
        firstName: document.querySelector("#firstName").value,
        lastName: document.querySelector("#lastName").value,
        email: document.querySelector("#email").value,
        phoneNumber: document.querySelector("#phoneNumber").value,
        address: document.querySelector("#address").value
    };

    await fetch(url, {
        method: method,
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(contact)
    });
    document.querySelector("#contact-form").reset();
    fetchContacts();
}

async function editContact(id) {
    const response = await fetch(`${apiUrl}/${id}`);
    const contact = await response.json();
    document.querySelector("#contact-id").value = contact.id;
    document.querySelector("#firstName").value = contact.firstName;
    document.querySelector("#lastName").value = contact.lastName;
    document.querySelector("#email").value = contact.email;
    document.querySelector("#phoneNumber").value = contact.phoneNumber || "";
    document.querySelector("#address").value = contact.address || "";
}

async function deleteContact(id) {
    await fetch(`${apiUrl}/${id}`, { method: "DELETE" });
    fetchContacts();
}

document.querySelector("#contact-form").addEventListener("submit", addOrUpdateContact);
fetchContacts();
