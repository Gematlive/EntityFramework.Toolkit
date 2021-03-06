﻿using System;
using System.Linq.Expressions;

using EntityFramework.Toolkit.EF6;

using ToolkitSample.Model;

namespace ToolkitSample.DataAccess.Seed
{
    internal sealed class CountryDataSeed : DataSeedBase<Country>
    {
        public override Expression<Func<Country, object>> AddOrUpdateExpression
        {
            get
            {
                return applicationSetting => applicationSetting.Id;
            }
        }

        public override Country[] GetAll()
        {
            return new[]
            {
                new Country { Id = "AD", Name = "Andorra" },
                new Country { Id = "AE", Name = "United Arab Emirates" },
                new Country { Id = "AF", Name = "Afghanistan" },
                new Country { Id = "AG", Name = "Antigua and Barbuda" },
                new Country { Id = "AL", Name = "Albania" },
                new Country { Id = "AM", Name = "Armenia" },
                new Country { Id = "AO", Name = "Angola" },
                new Country { Id = "AR", Name = "Argentina" },
                new Country { Id = "AT", Name = "Austria" },
                new Country { Id = "AU", Name = "Australia" },
                new Country { Id = "AZ", Name = "Azerbaijan" },
                new Country { Id = "BA", Name = "Bosnia and Herzegovina" },
                new Country { Id = "BB", Name = "Barbados" },
                new Country { Id = "BD", Name = "Bangladesh" },
                new Country { Id = "BE", Name = "Belgium" },
                new Country { Id = "BF", Name = "Burkina Faso" },
                new Country { Id = "BG", Name = "Bulgaria" },
                new Country { Id = "BH", Name = "Bahrain" },
                new Country { Id = "BI", Name = "Burundi" },
                new Country { Id = "BJ", Name = "Benin" },
                new Country { Id = "BN", Name = "Brunei Darussalam" },
                new Country { Id = "BO", Name = "Bolivia (Plurinational State of)" },
                new Country { Id = "BR", Name = "Brazil" },
                new Country { Id = "BS", Name = "Bahamas" },
                new Country { Id = "BT", Name = "Bhutan" },
                new Country { Id = "BW", Name = "Botswana" },
                new Country { Id = "BY", Name = "Belarus" },
                new Country { Id = "BZ", Name = "Belize" },
                new Country { Id = "CA", Name = "Canada" },
                new Country { Id = "CD", Name = "Democratic Republic of the Congo" },
                new Country { Id = "CF", Name = "Central African Republic" },
                new Country { Id = "CG", Name = "Congo" },
                new Country { Id = "CH", Name = "Switzerland" },
                new Country { Id = "CI", Name = "Côte d'Ivoire" },
                new Country { Id = "CL", Name = "Chile" },
                new Country { Id = "CM", Name = "Cameroon" },
                new Country { Id = "CN", Name = "China" },
                new Country { Id = "CO", Name = "Colombia" },
                new Country { Id = "CR", Name = "Costa Rica" },
                new Country { Id = "CU", Name = "Cuba" },
                new Country { Id = "CV", Name = "Cape Verde" },
                new Country { Id = "CY", Name = "Cyprus" },
                new Country { Id = "CZ", Name = "Czech Republic" },
                new Country { Id = "DE", Name = "Germany" },
                new Country { Id = "DJ", Name = "Djibouti" },
                new Country { Id = "DK", Name = "Denmark" },
                new Country { Id = "DM", Name = "Dominica" },
                new Country { Id = "DO", Name = "Dominican Republic" },
                new Country { Id = "DZ", Name = "Algeria" },
                new Country { Id = "EC", Name = "Ecuador" },
                new Country { Id = "EE", Name = "Estonia" },
                new Country { Id = "EG", Name = "Egypt" },
                new Country { Id = "ER", Name = "Eritrea" },
                new Country { Id = "ES", Name = "Spain" },
                new Country { Id = "ET", Name = "Ethiopia" },
                new Country { Id = "FI", Name = "Finland" },
                new Country { Id = "FJ", Name = "Fiji" },
                new Country { Id = "FM", Name = "Micronesia (Federated States of)" },
                new Country { Id = "FR", Name = "France" },
                new Country { Id = "GA", Name = "Gabon" },
                new Country { Id = "GB", Name = "United Kingdom of Great Britain and Northern Ireland" },
                new Country { Id = "GD", Name = "Grenada" },
                new Country { Id = "GE", Name = "Georgia" },
                new Country { Id = "GH", Name = "Ghana" },
                new Country { Id = "GM", Name = "Gambia" },
                new Country { Id = "GN", Name = "Guinea" },
                new Country { Id = "GQ", Name = "Equatorial Guinea" },
                new Country { Id = "GR", Name = "Greece" },
                new Country { Id = "GT", Name = "Guatemala" },
                new Country { Id = "GW", Name = "Guinea-Bissau" },
                new Country { Id = "GY", Name = "Guyana" },
                new Country { Id = "HN", Name = "Honduras" },
                new Country { Id = "HR", Name = "Croatia" },
                new Country { Id = "HT", Name = "Haiti" },
                new Country { Id = "HU", Name = "Hungary" },
                new Country { Id = "ID", Name = "Indonesia" },
                new Country { Id = "IE", Name = "Ireland" },
                new Country { Id = "IL", Name = "Israel" },
                new Country { Id = "IN", Name = "India" },
                new Country { Id = "IQ", Name = "Iraq" },
                new Country { Id = "IR", Name = "Iran (Islamic Republic of)" },
                new Country { Id = "IS", Name = "Iceland" },
                new Country { Id = "IT", Name = "Italy" },
                new Country { Id = "JM", Name = "Jamaica" },
                new Country { Id = "JO", Name = "Jordan" },
                new Country { Id = "JP", Name = "Japan" },
                new Country { Id = "KE", Name = "Kenya" },
                new Country { Id = "KG", Name = "Kyrgyzstan" },
                new Country { Id = "KH", Name = "Cambodia" },
                new Country { Id = "KI", Name = "Kiribati" },
                new Country { Id = "KM", Name = "Comoros" },
                new Country { Id = "KN", Name = "Saint Kitts and Nevis" },
                new Country { Id = "KP", Name = "Democratic People's Republic of Korea" },
                new Country { Id = "KR", Name = "Republic of Korea" },
                new Country { Id = "KW", Name = "Kuwait" },
                new Country { Id = "KZ", Name = "Kazakhstan" },
                new Country { Id = "LA", Name = "Lao People's Democratic Republic" },
                new Country { Id = "LB", Name = "Lebanon" },
                new Country { Id = "LC", Name = "Saint Lucia" },
                new Country { Id = "LI", Name = "Liechtenstein" },
                new Country { Id = "LK", Name = "Sri Lanka" },
                new Country { Id = "LR", Name = "Liberia" },
                new Country { Id = "LS", Name = "Lesotho" },
                new Country { Id = "LT", Name = "Lithuania" },
                new Country { Id = "LU", Name = "Luxembourg" },
                new Country { Id = "LV", Name = "Latvia" },
                new Country { Id = "LY", Name = "Libyan Arab Jamahiriya" },
                new Country { Id = "MA", Name = "Morocco" },
                new Country { Id = "MC", Name = "Monaco" },
                new Country { Id = "MD", Name = "Republic of Moldova" },
                new Country { Id = "ME", Name = "Montenegro" },
                new Country { Id = "MG", Name = "Madagascar" },
                new Country { Id = "MH", Name = "Marshall Islands" },
                new Country { Id = "MK", Name = "The former Yugoslav Republic of Macedonia" },
                new Country { Id = "ML", Name = "Mali" },
                new Country { Id = "MM", Name = "Myanmar" },
                new Country { Id = "MN", Name = "Mongolia" },
                new Country { Id = "MR", Name = "Mauritania" },
                new Country { Id = "MT", Name = "Malta" },
                new Country { Id = "MU", Name = "Mauritius" },
                new Country { Id = "MV", Name = "Maldives" },
                new Country { Id = "MW", Name = "Malawi" },
                new Country { Id = "MX", Name = "Mexico" },
                new Country { Id = "MY", Name = "Malaysia" },
                new Country { Id = "MZ", Name = "Mozambique" },
                new Country { Id = "NA", Name = "Namibia" },
                new Country { Id = "NE", Name = "Niger" },
                new Country { Id = "NG", Name = "Nigeria" },
                new Country { Id = "NI", Name = "Nicaragua" },
                new Country { Id = "NL", Name = "Netherlands" },
                new Country { Id = "NO", Name = "Norway" },
                new Country { Id = "NP", Name = "Nepal" },
                new Country { Id = "NR", Name = "Nauru" },
                new Country { Id = "NZ", Name = "New Zealand" },
                new Country { Id = "OM", Name = "Oman" },
                new Country { Id = "PA", Name = "Panama" },
                new Country { Id = "PE", Name = "Peru" },
                new Country { Id = "PG", Name = "Papua New Guinea" },
                new Country { Id = "PH", Name = "Philippines" },
                new Country { Id = "PK", Name = "Pakistan" },
                new Country { Id = "PL", Name = "Poland" },
                new Country { Id = "PT", Name = "Portugal" },
                new Country { Id = "PW", Name = "Palau" },
                new Country { Id = "PY", Name = "Paraguay" },
                new Country { Id = "QA", Name = "Qatar" },
                new Country { Id = "RO", Name = "Romania" },
                new Country { Id = "RS", Name = "Serbia" },
                new Country { Id = "RU", Name = "Russian Federation" },
                new Country { Id = "RW", Name = "Rwanda" },
                new Country { Id = "SA", Name = "Saudi Arabia" },
                new Country { Id = "SB", Name = "Solomon Islands" },
                new Country { Id = "SC", Name = "Seychelles" },
                new Country { Id = "SD", Name = "Sudan" },
                new Country { Id = "SE", Name = "Sweden" },
                new Country { Id = "SG", Name = "Singapore" },
                new Country { Id = "SI", Name = "Slovenia" },
                new Country { Id = "SK", Name = "Slovakia" },
                new Country { Id = "SL", Name = "Sierra Leone" },
                new Country { Id = "SM", Name = "San Marino" },
                new Country { Id = "SN", Name = "Senegal" },
                new Country { Id = "SO", Name = "Somalia" },
                new Country { Id = "SR", Name = "Suriname" },
                new Country { Id = "SS", Name = "South Sudan" },
                new Country { Id = "ST", Name = "Sao Tome and Principe" },
                new Country { Id = "SV", Name = "El Salvador" },
                new Country { Id = "SY", Name = "Syrian Arab Republic" },
                new Country { Id = "SZ", Name = "Swaziland" },
                new Country { Id = "TD", Name = "Chad" },
                new Country { Id = "TG", Name = "Togo" },
                new Country { Id = "TH", Name = "Thailand" },
                new Country { Id = "TJ", Name = "Tajikistan" },
                new Country { Id = "TL", Name = "Timor-Leste" },
                new Country { Id = "TM", Name = "Turkmenistan" },
                new Country { Id = "TN", Name = "Tunisia" },
                new Country { Id = "TO", Name = "Tonga" },
                new Country { Id = "TR", Name = "Turkey" },
                new Country { Id = "TT", Name = "Trinidad and Tobago" },
                new Country { Id = "TV", Name = "Tuvalu" },
                new Country { Id = "TZ", Name = "United Republic of Tanzania" },
                new Country { Id = "UA", Name = "Ukraine" },
                new Country { Id = "UG", Name = "Uganda" },
                new Country { Id = "US", Name = "United States of America" },
                new Country { Id = "UY", Name = "Uruguay" },
                new Country { Id = "UZ", Name = "Uzbekistan" },
                new Country { Id = "VC", Name = "Saint Vincent and the Grenadines" },
                new Country { Id = "VE", Name = "Venezuela (Bolivarian Republic of)" },
                new Country { Id = "VN", Name = "Viet Nam" },
                new Country { Id = "VU", Name = "Vanuatu" },
                new Country { Id = "WS", Name = "Samoa" },
                new Country { Id = "YE", Name = "Yemen" },
                new Country { Id = "ZA", Name = "South Africa" },
                new Country { Id = "ZM", Name = "Zambia" },
                new Country { Id = "ZW", Name = "Zimbabwe" },
            };
        }
    }
}