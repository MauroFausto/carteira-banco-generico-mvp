import { bestSellingProducts } from "./bestSellingProducts";
import { deviceSessionsEcommerce } from "./deviceSessionsEcommerce";
import { mostRecentSales } from "./mostRecentSales";
import { returningCustomerRate } from "./returningCustomerRate";
import { salesByRegions } from "./salesByRegions";
import { sessionsByReferrer } from "./sessionsByReferrer";
import { totalSales } from "./total-sales/totalSales";
import { totalOrders } from "./totalOrders";
import { storeConversionRate } from "./storeConversionRate";
import { activeStoreVisitors } from "./activeStoreVisitors";

export const eCommerce = () => {
	totalSales();
	returningCustomerRate();
	deviceSessionsEcommerce();
	salesByRegions();
	sessionsByReferrer();
	mostRecentSales();
	bestSellingProducts();
	totalOrders();
	storeConversionRate();
	activeStoreVisitors();
};
