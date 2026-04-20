export const bestSellingProducts = () => {
	const WRAPPER = document.getElementById("list-best-selling-products");
	let list = "";
	const DATA = [
		{
			item: "Sony CH510 Wireless Headphones",
			sales: 564,
			color: "teal",
			up: true,
			percentage: 83,
			change: 14.3,
		},
		{
			item: "Logitech C922 Pro Stream Webcam",
			sales: 456,
			color: "success",
			up: true,
			percentage: 71,
			change: 54.5,
		},
		{
			item: "SanDisk SDSSDE60-1T00-G25 1TB",
			sales: 400,
			color: "info",
			up: false,
			percentage: 58,
			change: 0.9,
		},
		{
			item: "HP X6W31AA 200 Wireless Mouse",
			sales: 399,
			color: "primary",
			up: false,
			percentage: 49,
			change: 65.7,
		},
		{
			item: "Samsung Galaxy S10 Lite Dual SIM 128GB",
			sales: 395,
			color: "orange",
			up: true,
			percentage: 32,
			change: 6.8,
		},
	];

	if (WRAPPER) {
		DATA.forEach((item, index) => {
			list += `<div class="d-flex align-items-center flex-wrap">
                        <div class="avatar avatar-xs d-grid place-content-center me-3 text-white fs-8 bg-${item.color}">
                            ${index + 1}
                        </div>

                        <div class="flex-grow-1 text-truncate pe-4">${item.item}</div>
                        <div class="fs-5 fw-medium">${item.sales}</div>

                        <div class="fs-7 d-none d-sm-flex align-items-center justify-content-end lh-1 w-16 ${item.up ? "text-success" : "text-danger"}">
                            ${item.change}%

                            <i class="ph fs-5 ms-1 ${item.up ? "ph-arrow-circle-up" : "ph-arrow-circle-down"}"></i>
                        </div>

                        <div class="progress w-100 my-4 h-0.5" role="progressbar" aria-valuenow="${item.percentage}" aria-valuemin="0" aria-valuemax="100">
							<div class="progress-bar progress-bar-striped progress-bar-animated bg-${item.color}" style="width: ${item.percentage}%"></div>
						</div>
                    </div>`;
		});

		WRAPPER.innerHTML = list;
	}
};
