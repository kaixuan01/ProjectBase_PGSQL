// routes.js
import { faDashboard, faGear, faList, faShield, faUser } from "@fortawesome/free-solid-svg-icons";
import Dashboard from "../View/dashboard";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import UserListing from "../View/Admin/User/UserListing";
const myURLRoutes = [
    {
      path: '/',
      name: 'Dashboard',
      component: <Dashboard/>,
      icon: <FontAwesomeIcon icon={faDashboard}/>,
      subRoutes: [],
    },
    {
      path: '/admin',
      name: 'Admin',
      component: <Dashboard/>,
      icon: <FontAwesomeIcon icon={faUser}/>,
      subRoutes: [
        {
          path: '/userListing',
          name: 'UserListing',
          icon: <FontAwesomeIcon icon={faList}/>,
          component: <UserListing/>,
        },
      ],
    },
    {
      path: '/settings',
      name: 'Settings',
      component: '',
      icon: <FontAwesomeIcon icon={faGear}/>,
      subRoutes: [
        {
          path: '/security',
          name: 'Security',
          component: '',
          icon: <FontAwesomeIcon icon={faShield}/>,

        },
      ],
    },
    {
      path: '/downloadSomething',
      name: 'Download',
      component: '',
      excludedMenu: true,
    }
    // Add more routes as needed
  ];
  
  export default myURLRoutes;
  