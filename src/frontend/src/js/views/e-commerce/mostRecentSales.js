import images from "../../../img/products/*.jpg";
const WRAPPER = document.getElementById("list-most-recent-sales");
let list = "";

export const mostRecentSales = () => {
	const DATA = [
		{
			id: 34532,
			image: "1",
			name: "2020 Apple iPhone SE 128GB",
			date: "2 mins ago",
			price: 350.0,
			up: false,
			change: 23.3,
		},
		{
			id: 43545,
			image: "2",
			name: "Samsung Galaxy Watch 42mm LTE",
			date: "5 mins ago",
			price: 299.99,
			up: true,
			change: 56.2,
		},
		{
			id: 65342,
			image: "3",
			name: "Das Mechanical Keyboard Black",
			date: "15 mins ago",
			price: 150.0,
			up: true,
			change: 54.6,
		},
		{
			id: 34532,
			image: "4",
			name: "Sone CH510 Wireless Headphones ",
			date: "22 mins ago",
			price: 12.01,
			up: true,
			change: 98.2,
		},
		{
			id: 34532,
			image: "5",
			name: "Leica Classic Camera Mirrorless",
			date: "33 mins ago",
			price: 1499.99,
			up: false,
			change: 78.4,
		},
		{
			id: 34532,
			image: "6",
			name: "Sony PS4 Controller",
			date: "An hour ago",
			price: 50.0,
			up: true,
			change: 99.9,
		},
	];

	if (WRAPPER) {
		DATA.forEach((item, index) => {
			list += `<div class="d-flex align-items-start gap-3 pt-3 ${index !== DATA.length - 1 ? "border-bottom pb-3" : ""}">
                        <img class="w-14 rounded-1" src="${images[item.image]}" alt="${item.name}" />
                        <div class="flex-grow-1 text-truncate">
                            <div class="text-truncate mb-0.5">${item.name}</div>
                            <div class="fs-8 text-body-secondary">
                                <span class="d-none d-sm-inline">${item.id} — </span>${item.price} — ${item.date}
                            </div>
                        </div>
                        <div class="fs-7 d-none d-sm-flex align-items-center justify-content-end lh-1 ${item.up ? "text-success" : "text-danger"}">
                            ${item.change}%

                            <i class="ph fs-5 ms-1 ${item.up ? "ph-arrow-circle-up" : "ph-arrow-circle-down"}"></i>
                        </div>
                    </div>`;
		});

		WRAPPER.innerHTML = list;
	}
};
