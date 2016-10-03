<?xml version="1.0" encoding="UTF-8"?>
<?xml-stylesheet type="text/xsl" href="log.xsl"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format">
  <xsl:template match="entry">
    <font size="2" face="MS Sans Serif">
      <xsl:choose>
        <xsl:when test="@type='Information'">
          <h4 style="background-color: #00FFFF; margin-bottom: 0">
            <xsl:value-of select="@type" />
            <xsl:text>: </xsl:text>
            <xsl:value-of select="@message" />
          </h4>
        </xsl:when>
        <xsl:when test="@type='Warning'">
          <h4 style="background-color: #FFFF00; margin-bottom: 0">
            <xsl:value-of select="@type" />
            <xsl:text>: </xsl:text>
            <xsl:value-of select="@message" />
          </h4>
        </xsl:when>
        <xsl:when test="@type='Error'">
          <h4 style="background-color: #FF7D7D; margin-bottom: 0">
            <xsl:value-of select="@type" />
            <xsl:text>: </xsl:text>
            <xsl:value-of select="@message" />
          </h4>
        </xsl:when>
      </xsl:choose>
      <p style="line-height: 100%; margin-top: 0; margin-bottom: 1">
        <b>
          <xsl:text>Date: </xsl:text>
        </b>
        <xsl:value-of select="@date" />
        <b>
          <xsl:text> Time: </xsl:text>
        </b>
        <xsl:value-of select="@time" />
        <b>
          <xsl:text> Source: </xsl:text>
        </b>
        <xsl:value-of select="@source" />
      </p>
      <xsl:apply-templates />
    </font>
  </xsl:template>
  <xsl:template match="operation">
    <p style="line-height: 100%; margin-top: 0; margin-bottom: 1">
      <b>
        <xsl:text>Operation: </xsl:text>
      </b>
      <xsl:value-of select="@name" />
    </p>
  </xsl:template>
  <xsl:template match="parameter">
    <p style="line-height: 100%; margin-top: 0; margin-bottom: 1">
      <b>
        <xsl:text>Parameter </xsl:text>
      </b>
      <xsl:value-of select="@name" />
      <b>
        <xsl:text> = </xsl:text>
      </b>
      <xsl:value-of select="@value" />
    </p>
  </xsl:template>
  <xsl:template match="exception">
    <p style="line-height: 100%; margin-top: 0; margin-bottom: 1">
      <b>
        <xsl:text>Exception type: </xsl:text>
      </b>
      <xsl:value-of select="@type" />
    </p>
    <p style="line-height: 100%; margin-top: 0; margin-bottom: 1">
      <b>
        <xsl:text>The exception message: </xsl:text>
      </b>
      <xsl:value-of select="@message" />
    </p>
  </xsl:template>
  <xsl:template match="additional">
    <p style="line-height: 100%; margin-top: 0; margin-bottom: 1">
      <b>
        <xsl:text>Additional information: </xsl:text>
      </b>
      <xsl:value-of select="@message" />
    </p>
  </xsl:template>
  <xsl:template match="user">
    <p style="line-height: 100%; margin-top: 0; margin-bottom: 1">
      <b>
        <xsl:text>User name: </xsl:text>
      </b>
      <xsl:value-of select="@name" />
      <b>
        <xsl:text>  User id: </xsl:text>
      </b>
      <xsl:value-of select="@id" />
    </p>
  </xsl:template>
  <xsl:template match="memory">
    <p style="line-height: 100%; margin-top: 0; margin-bottom: 1">
      <b>
        <xsl:text>Memory Usage: </xsl:text>
      </b>
      <xsl:value-of select="@memory" />
      <b>
        <xsl:text>  Virtual Memory Usage: </xsl:text>
      </b>
      <xsl:value-of select="@virtualMemory" />
    </p>
  </xsl:template>
</xsl:stylesheet>