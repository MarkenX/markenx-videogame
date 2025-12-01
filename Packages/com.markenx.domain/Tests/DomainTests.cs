using NUnit.Framework;
using System.Collections.Generic;
using System;

namespace MarkenX.Domain.Tests

{
    public class DomainTests
    {
        private DimensionDefinition _priceDef;
        private DimensionDefinition _qualityDef;
        private DimensionDefinition _socialDef;

        [SetUp]
        public void Setup()
        {
            // Se ejecuta antes de CADA test para limpiar variables
            _priceDef = new DimensionDefinition("PriceSensitivity", "Sensitivity to price");
            _qualityDef = new DimensionDefinition("Quality", "Quality expectation");
            _socialDef = new DimensionDefinition("Social", "Social recognition need");
        }

        #region Pruebas de Modelos (ProductProfile & Actions)

        [Test]
        public void ProductProfile_SetAndGet_ShouldStoreCorrectValue()
        {
            // 1. Arrange
            var profile = new ProductProfile();

            // 2. Act
            profile.Set(_priceDef, 0.8f);
            var result = profile.Get(_priceDef);

            // 3. Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0.8f, result.Value);
        }

        [Test]
        public void DimensionValue_Clamping_ShouldLimitValuesBetween0And1()
        {
            var val = new DimensionValue(_priceDef, 1.5f);
            Assert.AreEqual(1.0f, val.Value, "El valor debería limitarse a 1.0 máx");

            val.Add(-2.0f);
            Assert.AreEqual(0.0f, val.Value, "El valor debería limitarse a 0.0 mín");
        }

        [Test]
        public void MarketAction_Apply_ShouldModifyProfile()
        {
            // 1. Arrange: Producto base con precio bajo (0.2)
            var product = new ProductProfile();
            product.Set(_priceDef, 0.2f);

            // Acción: "Subida de precios" (+0.5)
            var action = new MarketAction("Price Hike", "Testing", 0);
            action.AddEffect(_priceDef, 0.5f);

            // 2. Act
            action.Apply(product);

            // 3. Assert: 0.2 + 0.5 = 0.7
            Assert.AreEqual(0.7f, product.Get(_priceDef).Value, 0.001f);
        }

        #endregion

        #region Pruebas de Reglas (RuleEngine)

        [Test]
        public void RuleEngine_ApplyDemographics_YoungAge_ShouldIncreaseSocialRecognition()
        {
            var dni = new DNI("Joven", "Test", 20, 1000); 
            var consumer = new ConsumerProfile();

            RuleEngine.ApplyDemographicRules(dni, consumer, _priceDef, _socialDef);

            float socialValue = consumer.Get(_socialDef).Value;
            Assert.AreEqual(0.8f, socialValue, 0.01f, "Un joven de 20 años debería tener 0.8 en social");
        }

        [Test]
        public void RuleEngine_ApplyDemographics_LowIncome_ShouldIncreasePriceSensitivity()
        {
            var dni = new DNI("Broke", "Test", 30, 0); 
            var consumer = new ConsumerProfile();

            RuleEngine.ApplyDemographicRules(dni, consumer, _priceDef, _socialDef);

            float priceSens = consumer.Get(_priceDef).Value;
            Assert.AreEqual(1.0f, priceSens, 0.01f, "Ingresos 0 deberían dar sensibilidad máxima al precio");
        }

        #endregion

        #region Pruebas de Distancia (El corazón del Matchmaking)

        [Test]
        public void DistanceMetric_Manhattan_PerfectMatch_ShouldReturn1()
        {
            // Arrange
            var consumer = new ConsumerProfile();
            consumer.Set(_qualityDef, 0.8f);

            var product = new ProductProfile();
            product.Set(_qualityDef, 0.8f);

            // Act
            float score = DistanceMetric.ComputeAcceptance(consumer, product);

            // Assert
            Assert.AreEqual(1.0f, score, "Si los valores son idénticos, la aceptación debe ser 100%");
        }

        [Test]
        public void DistanceMetric_Manhattan_CompleteMismatch_ShouldReturn0()
        {
            // Arrange
            var consumer = new ConsumerProfile();
            consumer.Set(_qualityDef, 1.0f);

            var product = new ProductProfile();
            product.Set(_qualityDef, 0.0f);

            // Act
            float score = DistanceMetric.ComputeAcceptance(consumer, product);

            // Assert: Diferencia es 1.0. Score = 1 - 1 = 0.
            Assert.AreEqual(0.0f, score, "Si son opuestos, la aceptación debe ser 0%");
        }

        [Test]
        public void DistanceMetric_Weighted_ShouldPrioritizeImportantDimensions()
        {
            // Escenario: 
            // - Calidad: Coinciden (Diferencia 0) -> Pero tiene poco peso (0.1)
            // - Precio: Difieren mucho (Diferencia 1.0) -> Y tiene MUCHO peso (10.0)
            // El resultado debería ser muy malo porque falló en lo importante.

            var consumer = new ConsumerProfile();
            consumer.Set(_qualityDef, 1.0f);
            consumer.Set(_priceDef, 0.0f);

            var product = new ProductProfile();
            product.Set(_qualityDef, 1.0f);
            product.Set(_priceDef, 1.0f);

            var weights = new Dictionary<DimensionDefinition, float>
            {
                { _qualityDef, 0.1f },
                { _priceDef, 10.0f }
            };

            float score = DistanceMetric.ComputeWeightedAcceptance(consumer, product, weights);

            Assert.Less(score, 0.2f, "El score debería ser bajo porque falló en la dimensión de mayor peso");
        }

        #endregion
    }
}