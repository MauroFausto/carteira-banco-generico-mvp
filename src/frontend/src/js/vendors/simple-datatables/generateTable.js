export const generateTable = (table, rowCount, checkbox = false) => {
	const headers = [...(checkbox ? ["Select"] : []), "Name", "Country", "Ext", "Start Date", "Completions (%)"];
	const countries = [
		"USA",
		"India",
		"UK",
		"Canada",
		"Australia",
		"Germany",
		"France",
		"Italy",
		"Spain",
		"Japan",
		"China",
		"Brazil",
		"Russia",
		"South Africa",
		"Mexico",
		"Netherlands",
		"Sweden",
		"Norway",
		"Denmark",
		"Argentina",
		"South Korea",
		"Turkey",
		"Switzerland",
		"New Zealand",
		"Saudi Arabia",
	];

	// Generate data
	const data = Array.from({ length: rowCount }, () => ({
		Select: "",
		Name: `User ${Math.floor(Math.random() * 1000)}`,
		Country: countries[Math.floor(Math.random() * countries.length)],
		Ext: `${Math.floor(10000 + Math.random() * 90000)}`, // Random 5-digit extension
		"Start Date": `${String(Math.floor(1 + Math.random() * 28)).padStart(2, "0")}/${String(Math.floor(1 + Math.random() * 12)).padStart(2, "0")}/202${Math.floor(0 + Math.random() * 4)}`,
		"Completions (%)": `${Math.floor(50 + Math.random() * 50)}%`,
	}));

	// Create thead
	const thead = document.createElement("thead");
	const headerRow = document.createElement("tr");
	headers.forEach((header) => {
		const th = document.createElement("th");
		th.textContent = header;
		headerRow.appendChild(th);
	});
	thead.appendChild(headerRow);
	table.appendChild(thead);

	// Create tbody
	const tbody = document.createElement("tbody");
	data.forEach((row) => {
		const tr = document.createElement("tr");
		headers.forEach((header) => {
			const td = document.createElement("td");
			td.textContent = row[header];
			tr.appendChild(td);
		});
		tbody.appendChild(tr);
	});
	table.appendChild(tbody);
};
