async function loadIntoTable(url, table){
    const tableHead = table.querySelector("THead");
    const tableBody = table.querySelector("TBody");
    const response = await fetch(url);
    const {headers, rows} = await response.json();

    //console.log(data);
    tableHead.innerHTML = "<tr></tr>";
    tableBody.innerHTML = "";

    for (const headerText of headers){
        const headerElement = document.createElement("th");

        headerElement.textContent = headerText;
        tableHead.querySelector("tr").appendChild(headerElement);
    }
    
    
}
loadIntoTable("https://dis02.dexon-technology.com/api/dexon-exam/get-data-list", document.querySelector("table"))
