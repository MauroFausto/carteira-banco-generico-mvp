import { generateTable } from "./generateTable";
import { DataTable } from "simple-datatables";

export const tableBasic = () => {
	const TABLE_BASIC = document.getElementById("datatable-basic");

	if (TABLE_BASIC) {
		// Generate table
		generateTable(TABLE_BASIC, 100);

		// Initiate datatable
		setTimeout(() => {
			let dataTable = new DataTable(TABLE_BASIC);
		});
	}
};
