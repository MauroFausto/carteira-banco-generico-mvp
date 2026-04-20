import themeThumbnails from "../../../img/theme/*.jpg";

import { activeUsersUpdate } from "../../views/analytics/activeUsers";
import { visitDurationUpdate } from "../../views/analytics/averageVisitDurarion";
import { sessionsByCountryUpdate } from "../../views/analytics/sessionsByCountry";
import { userAcquisitionUpdate } from "../../views/analytics/userAcquisition";
import { chartsUpdate } from "../../views/charts/charts";
import { convertedLeadsUpdate } from "../../views/crm/convertedLeads";
import { leadsGenerationRateUpdate } from "../../views/crm/leadsGenerationRate";
import { leadsSourcesChartUpdate } from "../../views/crm/leadsSources";
import { outboundCallsUpdate } from "../../views/crm/outboundCalls";
import { resolutionByChannelUpdate } from "../../views/crm/resolutionByChannel";
import { campaignSalesUpdate } from "../../views/email-marketing/campaignSales";
import { deliveredVsOpenUpdate } from "../../views/email-marketing/deliveredVsOpen";
import { sentVsNotSentUpdate } from "../../views/email-marketing/sentVsNotSent";
import { budgetExpensesUpdate } from "../../views/project-management/budgetExpenses";
import { budgetUtilizationUpdate } from "../../views/project-management/budgetUtilization";
import { plannedVsActualChartUpdate } from "../../views/project-management/plannedVsActual";
import { projectsByStatusUpdate } from "../../views/project-management/projectsByStatus";
import { ticketsReopenedUpdate } from "../../views/project-management/ticketsReopened";
import { salesHistoryUpdate } from "../../views/sales/salesHistory";
import { storeSessionsUpdate } from "../../views/sales/storeSessions";

export const theme = () => {
	const WRAPPER = document.getElementById("top-themes");
	const ROOT = document.documentElement;
	let activeThemeId = localStorage.getItem("sa-theme") || window.defaultTheme;

	if (WRAPPER) {
		const DATA = [
			{
				id: 8,
				name: "Interstellar",
				new: true,
			},
			{
				id: 1,
				name: "Solaris",
			},
			{
				id: 2,
				name: "Galaxy",
			},
			{
				id: 3,
				name: "Forest",
			},
			{
				id: 4,
				name: "Desert",
			},
			{
				id: 5,
				name: "Northern Light",
			},
			{
				id: 6,
				name: "Sunset",
			},
			{
				id: 7,
				name: "Kinetic",
			},
		];

		DATA.map((theme, index) => {
			// Create theme preview thumbnails
			let item = document.createElement("button");
			item.type = "button";
			item.setAttribute("class", "d-block overflow-hidden position-relative");
			item.setAttribute("data-sa-theme-id", theme.id);
			item.setAttribute("data-new", theme.new ? "true" : "false");
			item.innerHTML = `<img src="${themeThumbnails[theme.id]}" class="object-fit-cover rounded h-20 w-100" alt="${theme.name}" />
                                <span class="position-absolute bottom-0 end-0 lh-2 py-2 px-4 text-white text-opacity-75">${theme.name}</span>`;

			// Add active class to the current theme preview
			if (activeThemeId == theme.id) {
				item.classList.add("active");
			}

			// Add click events
			item.onclick = () => {
				ROOT.setAttribute("data-sa-theme", theme.id);
				document.querySelectorAll("#top-themes button").forEach((btn) => btn.classList.remove("active"));
				item.classList.add("active");

				// Set the active theme in localStorage
				// So the next time the page loads, it will load the same theme
				localStorage.setItem("sa-theme", theme.id);

				// Update charts so that they reflect the new theme colors
				budgetExpensesUpdate();
				budgetUtilizationUpdate();
				projectsByStatusUpdate();
				ticketsReopenedUpdate();
				plannedVsActualChartUpdate();
				leadsGenerationRateUpdate();
				convertedLeadsUpdate();
				leadsSourcesChartUpdate();
				resolutionByChannelUpdate();
				outboundCallsUpdate();
				visitDurationUpdate();
				sessionsByCountryUpdate();
				userAcquisitionUpdate();
				activeUsersUpdate();
				salesHistoryUpdate();
				storeSessionsUpdate();
				campaignSalesUpdate();
				sentVsNotSentUpdate();
				deliveredVsOpenUpdate();
				chartsUpdate();
			};

			WRAPPER.appendChild(item);
		});
	}
};
