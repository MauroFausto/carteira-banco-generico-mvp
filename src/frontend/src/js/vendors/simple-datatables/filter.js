import { generateTable } from "./generateTable";
import { DataTable } from "simple-datatables";

export const tableFilter = () => {
	const TABLE_FILTER = document.getElementById("datatable-filter");

	if (TABLE_FILTER) {
		// Generate table
		generateTable(TABLE_FILTER, 100);

		// Initiate datatable
		setTimeout(() => {
			window.dt = new DataTable(TABLE_FILTER, {
				perPageSelect: [10, 50, ["All", -1]],
				columns: [
					{
						select: 2,
						sortSequence: ["desc", "asc"],
					},
					{
						select: 3,
						sortSequence: ["desc"],
					},
				],
				tableRender: (_data, table, type) => {
					if (type === "print") {
						return table;
					}
					const tHead = table.childNodes[0];
					const filterHeaders = {
						nodeName: "TR",
						childNodes: tHead.childNodes[0].childNodes.map((_th, index) => ({
							nodeName: "TH",
							childNodes: [
								{
									nodeName: "INPUT",
									attributes: {
										class: "datatable-input",
										type: "search",
										"data-columns": `[${index}]`,
										placeHolder: "Search...",
									},
								},
							],
						})),
					};
					tHead.childNodes.push(filterHeaders);
					return table;
				},
			});
		});
	}
};
