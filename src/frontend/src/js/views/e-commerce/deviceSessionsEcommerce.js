export const deviceSessionsEcommerce = () => {
	const WRAPPER = document.getElementById("list-device-sessions-ecommerce");
	let list = "";
	const DATA = [
		{
			icon: "ph-laptop",
			percentage: 76,
			sessions: 8564,
			bg: "bg-info",
			text: "text-info",
		},
		{
			icon: "ph-device-mobile-camera",
			percentage: 52,
			sessions: 2435,
			bg: "bg-purple",
			text: "text-purple",
		},
		{
			icon: "ph-device-tablet-speaker",
			percentage: 18,
			sessions: 321,
			bg: "bg-primary",
			text: "text-primary",
		},
	];

	if (WRAPPER) {
		DATA.forEach((item) => {
			list += `<div style="--bs-bg-opacity: 0.2" class="flex-grow-1 rounded d-flex align-items-center py-1 mb-5 ${item.bg + " " + item.text}">
                    <i class="ph fs-4 mx-3 ${item.icon}"></i>
                    <div class="flex-grow-1 d-flex align-items-center">
                        <div class="h-8 d-flex align-items-center justify-content-end rounded-1 bg-current" style="width: ${item.percentage + "%"}">
                            ${item.percentage > 10 ? `<div class="text-white fs-8 px-2">${item.percentage}%</div>` : ""}
                        </div>
                        ${item.percentage <= 9 ? `<div class="text-current fs-8 px-2">${item.percentage}%</div>` : ""}
                    </div>
                </div>`;
		});

		WRAPPER.innerHTML = list;
	}
};
