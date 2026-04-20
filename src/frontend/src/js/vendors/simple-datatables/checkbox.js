import { generateTable } from "./generateTable";
import { DataTable } from "simple-datatables";

export const tableCheckbox = () => {
	const TABLE_CHECKBOX = document.getElementById("datatable-toggle");

	if (TABLE_CHECKBOX) {
		// Generate table
		generateTable(TABLE_CHECKBOX, 100, true);

		// Initiate datatable

		const datatable = new DataTable(TABLE_CHECKBOX, {
			rowRender: (rowValue, tr, _index) => {
				if (!tr.attributes) {
					tr.attributes = {};
				}
				tr.attributes["data-name"] = rowValue.cells[1].data[0].data;
				return tr;
			},
			columns: [
				{
					select: 0,
					sortable: false,
					render: (value, _td, _rowIndex, _cellIndex) =>
						`<span class="checkbox fs-2 ph ${value ? "ph-square" : "ph-check-square text-highlight"}"></span>`,
				},
			],
		});

		datatable.dom.addEventListener("click", (event) => {
			if (event.target.matches("span.checkbox")) {
				event.preventDefault();
				event.stopPropagation();
				const name = event.target.parentElement.parentElement.dataset.name;
				const index = parseInt(event.target.parentElement.parentElement.dataset.index, 10);
				const row = datatable.data.data[index];
				const cell = row.cells[0];
				const checked = cell.data;
				cell.data = !checked;
				datatable.update();

				// Timeout used for the checkbox to have time to update and show as checked. It is not required.
				setTimeout(() => alert(`"${name}" has been ${checked ? "checked" : "unchecked"}.`), 10);
			}
		});
		window.datatable = datatable;
	}
};
