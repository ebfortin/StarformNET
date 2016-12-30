
namespace DLS.StarformNET.Display
{
    using System;
    using System.Linq;
    using System.Text;
    using Data;
    using System.Collections.Generic;

    public static class PlanetText
    {
        public static string GetSystemText(List<Planet> planets, ChemTable[] gases)
        {
            var sb = new StringBuilder();
            var sun = planets[0].Star;// head.Star;
            sb.AppendLine(GetSunText(sun, gases));
            sb.AppendLine();

            foreach (var p in planets)
            {
                sb.AppendLine(GetPlanetText(p, gases));
                sb.AppendLine();
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public static string GetSunText(Star star, ChemTable[] gases)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Star");
            sb.AppendLine("=========================");
            sb.AppendFormat("Luminosity: {0:0.00}", star.Luminosity);
            sb.AppendLine();
            sb.AppendFormat("Mass: {0:0.00} SM", star.Mass);
            sb.AppendLine();
            sb.AppendFormat("Age: {0:E2} years", star.Age);
            return sb.ToString();
        }

        public static string GetPlanetText(Planet planet, ChemTable[] gases)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} {1}", GetPlanetNumber(planet), GetPlanetTypeText(planet));
            sb.AppendLine();
            sb.AppendLine("-------------------------");
            sb.AppendLine();
            sb.AppendFormat("Orbital Distance: {0}\n", GetOrbitalDistanceAU(planet));
            sb.AppendLine();
            sb.AppendFormat("Equatorial Radius: {0}\n", GetRadiusKM(planet));
            sb.AppendLine();
            sb.AppendFormat("Surface Gravity: {0}\n", GetSurfaceGravityG(planet));
            sb.AppendLine();
            sb.AppendFormat("Escape Velocity: {0}\n", GetEscapeVelocity(planet));
            sb.AppendLine();
            sb.AppendFormat("Mass: {0}\n", GetMassStringEM(planet));
            sb.AppendLine();
            sb.AppendFormat("Length of Year: {0}\n", GetOrbitalPeriodDay(planet));
            sb.AppendLine();
            sb.AppendFormat("Length of Day: {0}\n", GetLengthofDayHours(planet));
            sb.AppendLine();
            sb.AppendFormat("Average Day Temperature: {0}\n", GetDayTemp(planet));
            sb.AppendLine();
            sb.AppendFormat("Average Night Temperature: {0}\n", GetNightTemp(planet));
            sb.AppendLine();
            sb.AppendFormat("Boiling Point: {0}\n", GetBoilingPoint(planet));
            sb.AppendLine();
            sb.AppendFormat("Greenhouse Rise: {0}\n", GetGreenhouseRise(planet));
            sb.AppendLine();
            sb.AppendFormat("Water Cover: {0}\n", GetHydrosphere(planet));
            sb.AppendLine();
            sb.AppendFormat("Ice Cover: {0}\n", GetIceCover(planet));
            sb.AppendLine();
            sb.AppendFormat("Cloud Cover: {0}\n", GetCloudCover(planet));
            sb.AppendLine();
            sb.AppendFormat("Surface Pressure: {0}\n", GetSurfacePressureStringAtm(planet));
            sb.AppendLine();
            sb.AppendFormat("Atmospheric Composition (Percentage): {0}\n", GetAtmoString(planet, gases));
            sb.AppendLine();
            sb.AppendFormat("Atmospheric Composition (Partial Pressure): {0}\n", GetAtmoStringPP(planet, gases));

            return sb.ToString();
        }

        private static string GetBoilingPoint(Planet planet)
        {
            return String.Format("{0:0.00} F", UnitConversions.KelvinToFahrenheit(planet.BoilingPointWater));
        }

        private static string GetGreenhouseRise(Planet planet)
        {
            return String.Format("{0:0.00} F", UnitConversions.KelvinToFahrenheit(planet.GreenhouseRise));
        }

        private static string GetEscapeVelocity(Planet planet)
        {
            return String.Format("{0:0.00} km/sec", UnitConversions.CMToKM(planet.EscapeVelocity));
        }

        private static string GetPlanetTypeText(Planet planet)
        {
            var sb = new StringBuilder();
            switch (planet.Type)
            {
                case PlanetType.GasGiant:
                    sb.Append("Gas Giant");
                    break;
                case PlanetType.SubGasGiant:
                    sb.Append("Small Gas Giant");
                    break;
                case PlanetType.SubSubGasGiant:
                    sb.Append("Gas Dwarf");
                    break;
                default:
                    sb.Append(planet.Type);
                    break;
            }
            if (planet.IsTidallyLocked)
            {
                sb.Append(", Tidally Locked");
            }
            if (planet.SurfPressure > 0 && planet.HasGreenhouseEffect)
            {
                sb.Append(", Runaway Greenhouse Effect");
            }
            switch (planet.breathability)
            {
                case Breathability.Breathable:
                case Breathability.Unbreathable:
                case Breathability.Poisonous:
                    sb.AppendFormat(", {0} Atmosphere", planet.breathability);
                    break;
                default:
                    sb.Append(", No Atmosphere");
                    break;
            }
            if (planet.IsEarthlike)
            {
                sb.Append(", Earthlike");
            }
            return sb.ToString();
        }

        private static string GetMoonCount(Planet planet)
        {
            var moons = 0;
            var moon = planet.FirstMoon;
            while (moon != null)
            {
                moons++;
                moon = moon.NextPlanet;
            }
            return moons.ToString();
        }

        private static string GetSurfaceGravityG(Planet planet)
        {
            if (planet.Type == PlanetType.GasGiant || planet.Type == PlanetType.SubGasGiant || planet.Type == PlanetType.SubSubGasGiant)
            {
                return "Oh yeah";
            }
            return String.Format("{0:0.00} G", planet.SurfaceGravity);
        }

        static string GetHydrosphere(Planet planet)
        {
            return String.Format("{0:0.0}%", planet.WaterCover * 100);
        }

        static string GetIceCover(Planet planet)
        {
            return String.Format("{0:0.0}%", planet.IceCover * 100);
        }

        static string GetCloudCover(Planet planet)
        {
            return String.Format("{0:0.0}%", planet.CloudCover * 100);
        }

        static string GetDayTemp(Planet planet)
        {
            return String.Format("{0:0.0} F", UnitConversions.KelvinToFahrenheit(planet.DaytimeTemp));
        }

        static string GetNightTemp(Planet planet)
        {
            return String.Format("{0:0.0} F", UnitConversions.KelvinToFahrenheit(planet.NighttimeTemp));
        }

        static string GetLengthofDayHours(Planet planet)
        {
            if (planet.Day > 24 * 7)
            {
                return string.Format("{0:0.0} days ({1:0.0} hours)", planet.Day / 24, planet.Day);
            }
            return String.Format("{0:0.0} hours", planet.Day);
        }

        static string GetOrbitalPeriodDay(Planet planet)
        {
            if (planet.OrbitalPeriod > 365 * 1.5)
            {
                return String.Format("{0:0.00} ({0:0.0} days)", planet.OrbitalPeriod / 365, planet.OrbitalPeriod);
            }
            return String.Format("{0:0.0} days", planet.OrbitalPeriod);
        }

        static string GetOrbitalDistanceAU(Planet planet)
        {
            return String.Format("{0:0.00} AU", planet.SemiMajorAxisAU);
        }

        static string GetPlanetNumber(Planet planet)
        {
            return String.Format("{0}.", planet.Position);
        }

        static string GetRadiusKM(Planet planet)
        {
            return String.Format("{0:0} km", planet.Radius);
        }

        static string GetMassStringEM(Planet planet)
        {
            return String.Format("{0:0.00} EM", UnitConversions.SolarMassesToEarthMasses(planet.Mass));
        }

        static string GetSurfacePressureStringAtm(Planet planet)
        {
            if (planet.Type == PlanetType.GasGiant || planet.Type == PlanetType.SubGasGiant || planet.Type == PlanetType.SubSubGasGiant)
            {
                return "Uh, a lot";
            }
            return String.Format("{0:0.000} atm", UnitConversions.MillibarsToAtm(planet.SurfPressure));
        }

        static string GetAtmoStringPP(Planet planet, ChemTable[] gases)
        {
            if (planet.Type == PlanetType.GasGiant || planet.Type == PlanetType.SubGasGiant || planet.Type == PlanetType.SubSubGasGiant)
            {
                return "Yes";
            }
            if (planet.GasCount == 0)
            {
                return "None";
            }
            var str = "";
            var orderedGases = planet.AtmosphericGases.OrderByDescending(g => g.surf_pressure).ToArray();
            if (orderedGases.Length == 0)
            {
                return "Trace gases only";
            }
            for (var i = 0; i < orderedGases.Length; i++)
            {
                var gas = orderedGases[i];
                var curGas = gas.GasType;
                str += String.Format("{0} [{1:0.0000} mb]", curGas.symbol, gas.surf_pressure);
                if (i < orderedGases.Length - 1)
                {
                    str += ", ";
                }
            }
            return str;
        }

        static string GetAtmoString(Planet planet, ChemTable[] gases, double minFraction = 0.01)
        {
            if (planet.Type == PlanetType.GasGiant || planet.Type == PlanetType.SubGasGiant || planet.Type == PlanetType.SubSubGasGiant)
            {
                return "Yes";
            }
            if (planet.GasCount == 0)
            {
                return "None";
            }
            var str = "";
            var orderedGases = planet.AtmosphericGases.Where(g => ((g.surf_pressure / planet.SurfPressure) * 100) > minFraction).OrderByDescending(g => g.surf_pressure).ToArray();
            if (orderedGases.Length == 0)
            {
                return "Trace gases only";
            }
            for (var i = 0; i < orderedGases.Length; i++)
            {
                var gas = orderedGases[i];
                var curGas = gas.GasType;
                var pct = (gas.surf_pressure / planet.SurfPressure) * 100;
                str += String.Format("{0} [{1:0.00}%]", curGas.symbol, pct);
                if (i < orderedGases.Length - 1)
                {
                    str += ", ";
                }
            }
            return str;
        }
    }
}
