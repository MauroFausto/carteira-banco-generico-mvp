import { generateTable } from "./generateTable";
import { DataTable } from "simple-datatables";

export const tableScroll = () => {
	const TABLE_SCROLL = document.getElementById("datatable-scroll");

	if (TABLE_SCROLL) {
		// Generate table
		generateTable(TABLE_SCROLL, 100);

		// Initiate datatable
		setTimeout(() => {
			let dataTable = new DataTable(TABLE_SCROLL, {
				paging: false,
				scrollY: "30vh",
				rowNavigation: true,
				tabIndex: 1,
			});
		});
	}
};
