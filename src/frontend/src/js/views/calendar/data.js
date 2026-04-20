const CURRENT_MONTH = new Date().getMonth() + 1;
const CURRENT_YEAR = new Date().getFullYear();
const FORMATTED_MONTH = CURRENT_MONTH < 10 ? "0" + CURRENT_MONTH : CURRENT_MONTH;

export const DATA = [
	{
		id: 1,
		title: "Deserunt aliuipsit",
		start: `${CURRENT_YEAR}-${FORMATTED_MONTH}-07T02:00:00`,
		allDay: true,
	},
	{
		id: 2,
		title: "Exercitation quis",
		start: `${CURRENT_YEAR}-${FORMATTED_MONTH}-04T18:30:00`,
		allDay: true,
	},
	{
		id: 3,
		title: "Amet pariatur cillum",
		start: `${CURRENT_YEAR}-${FORMATTED_MONTH}-06T12:00:00`,
	},
	{
		id: 4,
		title: "Laborum voluptate",
		start: `${CURRENT_YEAR}-${FORMATTED_MONTH}-06T08:00:00`,
		allDay: true,
	},
	{
		id: 5,
		title: "Fugiat dolor commodo",
		start: `${CURRENT_YEAR}-${FORMATTED_MONTH}-09`,
	},
	{
		id: 5,
		title: "Eiusmod adipisicing dolor",
		start: `${CURRENT_YEAR}-${FORMATTED_MONTH}-10`,
		allDay: true,
	},
	{
		id: 6,
		title: "Cilliom dendoom lowers",
		start: `${CURRENT_YEAR}-${FORMATTED_MONTH}-10T10:30:00`,
		allDay: true,
	},
	{
		id: 7,
		title: "Veniam deserunt cupidatat",
		start: `${CURRENT_YEAR}-${FORMATTED_MONTH}-14T07:30:00`,
		allDay: true,
	},
	{
		id: 8,
		title: "Adipisicing pariatur magna",
		start: `${CURRENT_YEAR}-${FORMATTED_MONTH}-19`,
	},
	{
		id: 9,
		title: "Fugiat ullamco commodo",
		start: `${CURRENT_YEAR}-${FORMATTED_MONTH}-19`,
	},
	{
		id: 10,
		title: "Consectetur",
		start: `${CURRENT_YEAR}-${FORMATTED_MONTH}-19T07:30:00`,
	},
	{
		id: 11,
		title: "Reprehenderis",
		start: `${CURRENT_YEAR}-${FORMATTED_MONTH}-23`,
		allDay: true,
	},
	{
		id: 12,
		title: "Consequat adipisicing",
		start: `${CURRENT_YEAR}-${FORMATTED_MONTH}-25`,
	},
	{
		id: 13,
		title: "Pariatur esse",
		start: `${CURRENT_YEAR}-${FORMATTED_MONTH}-28`,
		allDay: true,
	},
	{
		id: 14,
		title: "Ipsum dollar",
		start: `${CURRENT_YEAR}-${FORMATTED_MONTH}-28`,
	},
];
