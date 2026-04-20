import jsVectorMap from "jsvectormap";
import "jsvectormap/dist/maps/world.js";
import { resizeMap } from "../../vendors/jsvectormap/jsVectorMap";
import { COLORS } from "../../utils";

export const activeStoreVisitors = () => {
	// ----------------------------------------------
	// Map
	// ----------------------------------------------
	const MAP_WRAPPER = document.getElementById("map-store-visitors");

	if (MAP_WRAPPER) {
		// Resize map on load
		resizeMap(MAP_WRAPPER);

		// Initiate map
		new jsVectorMap({
			selector: "#map-store-visitors",
			map: "world",
			draggable: false,
			zoomButtons: false,
			zoomOnScroll: false,
			regionStyle: {
				initial: {
					fill: "rgba(255,255,255,0.5)",
					fillOpacity: 1,
					stroke: COLORS.transparent,
					strokeWidth: 1,
				},
				hover: {
					fillOpacity: 0.95,
				},
			},
			backgroundColor: ["rgba(0,0,0,0)"],
			visualizeData: {
				scale: ["#fffaf6", "#fffaf6"],
				values: {
					US: 500,
					BR: 450,
					ZA: 400,
					IN: 350,
					AE: 300,
					RU: 280,
					AU: 260,
					FR: 200,
					ES: 190,
					PL: 150,
					SE: 120,
					ID: 110,
					JP: 90,
					EG: 45,
				},
			},
			onLoaded(map) {
				// Resize map on load
				window.addEventListener("resize", () => {
					// Set map size on window resize
					resizeMap(MAP_WRAPPER);

					setTimeout(() => {
						map.updateSize();
					});
				});

				// Add class to scaled regions
				Object.keys(map.dataVisualization._values).forEach(function (code) {
					const regionElement = map.regions[code].element.shape.node;
					regionElement.classList.add("jvm-region-scale");
				});
			},
		});
	}

	// ----------------------------------------------
	// DATA
	// ----------------------------------------------
	const LIST_WRAPPER = document.getElementById("list-store-visitors");
	let list = "";
	const LIST_DATA = [
		{
			page: "/phones/galaxy-s20-ultra-black-512gb-5g/N38205577A",
			visitors: 324,
			percentage: 37.8,
		},
		{
			page: "/storage/sandisk-cruzer-blade-flash-drive-64gb-multicolour/N11412247A",
			visitors: 256,
			percentage: 29.8,
		},
		{
			page: "/network/tp-link-450mbps-wireless-n-router-450-mbps-black/N12923478A",
			visitors: 102,
			percentage: 11.9,
		},
		{
			page: "/sound/jbl-water-resistant-portable-bluetooth-speaker",
			visitors: 86,
			percentage: 10.1,
		},
		{
			page: "/gaming/sony-dualshock-4-wireless-controller-for-playstation-4-black/N13035683A",
			visitors: 54,
			percentage: 6.3,
		},
	];

	if (LIST_WRAPPER) {
		LIST_DATA.forEach((item, index) => {
			list += `<div class="hstack pt-3 text-body-emphasis ${index !== LIST_DATA.length - 1 ? "border-bottom pb-3" : ""}">
                        <div class="flex-grow-1 text-truncate d-flex align-items-center gap-3">
                            <i class="ph ph-app-window fs-4"></i>
                            <a class="text-truncate text-body" href="">${item.page}</a>
                        </div>
                        <div class="fs-5 fw-medium text-end w-16 flex-shrink-0">${item.visitors}</div>
                    </div>`;
		});

		LIST_WRAPPER.innerHTML = list;
	}
};
